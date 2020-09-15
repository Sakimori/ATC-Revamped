using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject camera;

    [SerializeField]
    public GameObject target;

    public Invert invertX = Invert.Standard;
    public Invert invertY = Invert.Inverted;
    public Invert invertZoom = Invert.Standard;

    public int zoomMult = 4;

    public float offsetX = 0;
    public float offsetY = 0;

    [SerializeField]
    bool debugCam = false;

    public float distanceFromObject = 20;

    void Awake()
    {

    }

    private void Update()
    {
        if (debugCam)
        {
            float axisZoom = Input.GetAxisRaw("Mouse ScrollWheel");
            transform.Translate(new Vector3(0, axisZoom * (int)invertZoom * zoomMult));

            if (Input.GetMouseButton(1))
            {
                float axisX = Input.GetAxisRaw("Mouse X");
                float axisY = Input.GetAxisRaw("Mouse Y");


                Vector3 translation = new Vector3(axisX * (int)invertX, 0, axisY * (int)invertY); //Reads mouse movement, applies possible inversion
                transform.Translate(translation);
            }
        }
        else
        {
            //float axisZoom = Input.GetAxisRaw("Mouse ScrollWheel");  These two lines let user change zoom. Probably won't.
            //distanceFromObject += axisZoom;

            transform.position = target.transform.position - camera.transform.forward * distanceFromObject;
            transform.Translate(new Vector3(offsetX, 0, offsetY), Space.Self); //Slides camera around for UI re-centering
        }
    }
    // Update is called once per frame
}
