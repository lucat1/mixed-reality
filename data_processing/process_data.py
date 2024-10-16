import pandas as pd
import json
import re
import os

# 1 - Initial dataframe exploration
def load_and_process_data():
    datapath = "data_processing/data/raw/componentwise_dataset.xlsx"
    df = pd.read_excel(datapath, sheet_name="Export")

    # Define column names
    cols_en = [
        "Fault start date (MEL)", "Short text (MEL)", "Long text (MEL)", "Message number (MEL)",
        "Vehicle family (TPL)", "Vehicle type (TPL)", "Material (CO)", "Material reference (MAT)",
        "Material quantity ACTUAL", "Material / unit of measure (CO)", "WF screen symptom (MEL)",
        "WF-cause-sys-1 (MEL) (H)", "WF-cause-sys-2 (MEL) (H)", "WF-Cause-Sys-3 (MEL) (H)",
        "WF-Cause-Sys-4 (MEL) (H)", "WF-Cause-Sys-5 (MEL) (H)", "WF-cause-sys-6 (MEL) (H)",
        "WF cause failure type (MEL)", "WF-cause-fix.mass (1-3) (MEL)", "Multiple unit / EW (TPL)",
        "Functional location (TPL)", "DIN code (H)", "Design workstation (location) (MEL)"
    ]

    cols_interst = [
        "Fault start date (MEL)", "Short text (MEL)", "Long text (MEL)", "Message number (MEL)",
        "Material reference (MAT)", "WF cause failure type (MEL)", "WF-cause-fix.mass (1-3) (MEL)",
        "Multiple unit / EW (TPL)"
    ]

    # Limit data to 1594 rows and set relevant columns
    df = df.loc[:1594]
    df.columns = cols_en
    df = df[cols_interst]

    # Rename columns for better readability
    df.rename(columns={
        "Message number (MEL)": "log_number",
        "Multiple unit / EW (TPL)": "train_number",
        "Fault start date (MEL)": "fault_start_date",
        "Short text (MEL)": "problem",
        "Long text (MEL)": "maintenance_log",
        "Material reference (MAT)": "material_name",
        "WF cause failure type (MEL)": "cause_of_failure",
        "WF-cause-fix.mass (1-3) (MEL)": "maintenance_steps"
    }, inplace=True)

    # Data type conversions
    df["fault_start_date"] = pd.to_datetime(df["fault_start_date"], format="%Y-%m-%d %H:%M:%S")
    df["problem"] = df["problem"].astype(str)
    df["maintenance_log"] = df["maintenance_log"].astype(str)
    df["log_number"] = df["log_number"].astype(int)
    df["train_number"] = df["train_number"].astype(str)
    df["material_name"] = df["material_name"].astype(str)
    df["maintenance_steps"] = df["maintenance_steps"].astype(str)
    df["cause_of_failure"] = df["cause_of_failure"].astype(str)

    # Remove the prefix from the train number
    df["train_number"] = df["train_number"].str[10:]

    # Reorder columns
    df = df[["train_number", "problem", "log_number", "maintenance_log", "fault_start_date", 
             "material_name", "maintenance_steps", "cause_of_failure"]]

    return df

# 2 - Associate components to BODE code
def filter_and_add_bode_code(df):
    components_bode_dict = {
        'SICHERHEITSGLAS': '25-351-0224-301',
        'ZAHNRIEMENRAD': '25-865-0011-301',
        'DREHFALLE': '25-341-0119-101',
        'WAHLSCHALTER': '25-714-0034-301',
        'TÜRSTEUERUNG': '25-004-1019-301'
    }

    def search_material_reference(row, search_dict):
        for key in search_dict.keys():
            if key.lower() in row['material_name'].lower():
                return True
        return False

    filtered_df = df[df.apply(lambda row: search_material_reference(row, components_bode_dict), axis=1)]
    filtered_df = filtered_df.drop_duplicates()

    def add_bode_code(row, search_dict):
        for key, code in search_dict.items():
            if key.lower() in row['material_name'].lower() or row['maintenance_log'].lower():
                return code
        return None

    filtered_df['BODE code'] = filtered_df.apply(lambda row: add_bode_code(row, components_bode_dict), axis=1)

    return filtered_df

# 3 - Extract logs and save the data
def extract_and_save_logs(df):
    log_dict = {}

    def extract_logs(maintenance_log):
        log_pattern = r'(\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}) CET.*Tel\. \+(\d+ \d{2} \d{3} \d{2} \d{2}).*\*\s*(.*)'
        matches = re.findall(log_pattern, maintenance_log)
        
        logs = []
        for match in matches:
            log = {
                "date_time": match[0],
                "telephone": match[1],
                "comment": match[2].strip(),
            }
            logs.append(log)
        
        return logs

    for index, row in df.iterrows():
        logs = extract_logs(row['maintenance_log'])
        for i, log in enumerate(logs):
            key = f"issue_{int(row['log_number'])}_log_{i+1}"
            log_dict[key] = log

    df['maintenance_log'] = df['log_number'].map(lambda log_number: [f"issue_{int(log_number)}_log_{i+1}" for i in range(len(extract_logs(df.loc[df['log_number'] == log_number, 'maintenance_log'].values[0])))])
    
    # Save the filtered DataFrame to CSV
    df.to_csv("data_processing/data/interim/final_df2.csv", index=False)

    # Save logs to JSON
    json_file_path = 'data_processing/data/interim/maintenance_logs.json'
    with open(json_file_path, 'w') as json_file:
        json.dump(log_dict, json_file, indent=4)

def main():
    df = load_and_process_data()
    filtered_df = filter_and_add_bode_code(df)
    filtered_df.to_csv("data_processing/data/interim/final_df.csv", index=False)
    extract_and_save_logs(filtered_df)

if __name__ == "__main__":
    main()
