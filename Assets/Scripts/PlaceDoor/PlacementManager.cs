using UnityEngine;
using UnityEngine.SceneManagement;
using MixedReality.Toolkit.UX;
using UnityEngine.Assertions;


public class PlacementManager : MonoBehaviour
{
    // Door object to activate in the scene
    public GameObject door;
    // Prefab for anchor points
    public GameObject anchorPointsPrefab;
    private GameObject anchorPoints;
    public GameObject distance;
    public GameObject steps;
    public GameObject miniature;

    private DoorManager dm;
    private Distance d;

    private StepsManager sm;
    private MiniatureManager mm;

    private const string confirmPath = "UIContainer/ButtonGroup_32x32mm_H3/ButtonCollection/ConfirmButton";

    private Vector3 scale = Vector3.one;

    public Vector3 Scale() {
        return scale;
    }

    // Instantiate the anchor points to perform door placement
    public void CreateAnchorPoints()
    {
        // Get camera position
        Vector3 cameraPosition = Camera.main.transform.position;
        
        if(anchorPoints != null)
            Destroy(anchorPoints);
        // Instantiate anchor points
        anchorPoints = Instantiate(anchorPointsPrefab, cameraPosition + new Vector3(-0.057f, -0.03f, 0.623f), Quaternion.identity);
        
        // Activate confirm button 
        var pb = transform.Find(confirmPath).gameObject.GetComponent<PressableButton>();
        pb.enabled = true;
    }

    public void MoveDoor()
    {
        Assert.IsNotNull(anchorPoints);
        Debug.Log("Moving the door to the appropriate position");

        var plane = anchorPoints.transform.Find("Plane");
        
        // Postition door in the scene and activate it
        door.transform.position = plane.position;
        door.transform.rotation = plane.rotation * Quaternion.Euler(90, 0, 90);
        float planeWidth = plane.localScale.x;

        // Adjust door size
        Transform DoorScaleTrasform = door.transform.Find("DoorContainer/DoorScale");
        scale = new (planeWidth/0.16583f, planeWidth/0.16583f,planeWidth/0.16583f);
        DoorScaleTrasform.localScale = scale;

        // Adjust door position after scale change
        Transform DoorResetPosition = door.transform.Find("DoorContainer");
        DoorResetPosition.localPosition = Vector3.zero;
    }

    public void Back() {
        Debug.Log("[PlaceDoor] Going back to door placement");

        // Toggle the palacing menu itself
        gameObject.SetActive(true);
        // Toggle anchor points anchor points
        anchorPoints.SetActive(true);

        // Toggle the visibility of the door
        dm.Hide();
        d.Hide();
        sm.Hide();
        sm.Reset();
        mm.Hide();
    }

    public void Forward()
    {
        Debug.Log("[PlaceDoor] Moving to steps");
        MoveDoor();

        // Toggle the palacing menu itself
        gameObject.SetActive(false);
        // Toggle anchor points anchor points
        anchorPoints.SetActive(false);

        // Toggle the visibility of the door
        dm.Show();
        d.Show();
        sm.Show();
        mm.Show();

        // GET TIME end place door timer and start actial manteinance timer
        if(TimeTracker.Instance){
            TimeTracker.Instance.EndAction();
            TimeTracker.Instance.StartAction("challenge|navigate steps");
        }
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        dm = door.GetComponent<DoorManager>();
        d = distance.GetComponent<Distance>();
        sm = steps.GetComponent<StepsManager>();
        mm = miniature.GetComponent<MiniatureManager>();

        // GET TIME
        if(TimeTracker.Instance){
            if(TimeTracker.Instance.challengeOn){
                Debug.Log("place the doooooooooooor ####################################W");
                TimeTracker.Instance.StartAction("challenge|place the door");
            }
        }
    }
}
