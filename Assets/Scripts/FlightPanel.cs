using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightPanel : MonoBehaviour
{
    public AirplaneController linkedPlaneController;

    public Text statusText;
    public Text flightNumText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //statusText.text = linkedPlaneController.statusText;
    }

    public void SelectThisFlight()
    {
        CameraController cam = FindObjectOfType<CameraController>();
        cam.target = linkedPlaneController.gameObject;
    }

    void ResetPanel()
    {
        statusText.text = "No Flight";
        flightNumText.text = "XX0000";
    }

    public void AttachFlightToPanel(GameObject newPlane)
    {
        linkedPlaneController = newPlane.GetComponent<AirplaneController>();
        flightNumText.text = linkedPlaneController.flightNumber;
        statusText.text = linkedPlaneController.statusText;
    }
}
