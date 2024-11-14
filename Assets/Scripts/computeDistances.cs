using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveObjectCoordinates : MonoBehaviour
{

    public Transform transform;
    List<string> smallComponentsList;

    [System.Serializable]
    public class ObjectCoordinate
    {
        public string name;
        public Vector3 coordinates;
    }

    [System.Serializable]
    public class ObjectCoordinateList
    {
        public List<ObjectCoordinate> objects = new List<ObjectCoordinate>();
    }

    List<string> LoadStringsFromFile(string path)
    {
        List<string> strings = new List<string>();

        if (File.Exists(path))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(path);
            strings.AddRange(lines);
        }
        else
        {
            Debug.LogError("File not found at: " + path);
        }

        return strings;
    }

    void Start()
    {

        string filePath = Path.Combine(Application.streamingAssetsPath, "small_components.txt");
        smallComponentsList = LoadStringsFromFile(filePath);
        // Step 1: Create a list to store object coordinates
        ObjectCoordinateList objectCoordinates = new ObjectCoordinateList();

        // Step 2: Traverse the hierarchy and collect coordinates
        CollectCoordinates(transform, objectCoordinates);

        // Step 3: Serialize the list to a JSON string
        string json = JsonUtility.ToJson(objectCoordinates, true);

        // Step 4: Save the JSON string to a file
        string filePathSave = Path.Combine(Application.dataPath, "ObjectCoordinates.json");
        File.WriteAllText(filePathSave, json);

        Debug.Log("Coordinates saved to " + filePathSave);
    }

    void CollectCoordinates(Transform parent, ObjectCoordinateList coordinates)
    {
        // Add the current object's name and coordinates to the list
        if(smallComponentsList.Contains(parent.name))
        {
            coordinates.objects.Add(new ObjectCoordinate
            {
                name = parent.name,
                coordinates = parent.position
            });
        }

        // Recursively call this method for each child
        foreach (Transform child in parent)
        {
            CollectCoordinates(child, coordinates);
        }
    }
}
