using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    GameObject nextDestination;

    [SerializeField]
    GameObject planeBody;

    Jurisdiction jurisdiction;
    
    [SerializeField]
    int airspeedMin;
    
    [SerializeField]
    int airspeedMax;

    [SerializeField]
    float airspeedCurrent;

    [SerializeField]
    int airspeedAccel;

    [SerializeField]
    float rollMaxAngle;

    [SerializeField]
    float rollSpeedAccel;

    [SerializeField]
    float turnSpeed;

    bool inMotion;

    // Start is called before the first frame update
    void Start()
    {
        inMotion = true;
        transform.LookAt(nextDestination.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (inMotion)
        {
            float transStep = airspeedCurrent * Time.deltaTime;
            //transform.rotation = Quaternion.Euler
            //transform.LookAt(nextDestination.transform);
            transform.position = transform.position + planeBody.transform.forward*transStep;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == nextDestination)
        {
            Waypoint thisWaypoint = nextDestination.GetComponent<Waypoint>();
            if (!thisWaypoint.isEnd)
            {
                if (!(thisWaypoint.forcedSpeed == 0))
                {
                    StartCoroutine("ChangeSpeed", thisWaypoint.forcedSpeed);
                }
                nextDestination = thisWaypoint.nextWaypoint.GetComponent<Waypoint>().gameObject;
                StartCoroutine("ChangeRotation");
            }
            else
            {
                inMotion = false;
                nextDestination = gameObject;
            }
        }
    }

    IEnumerator ChangeSpeed(float targetSpeed)
    {
        float startSpeed = airspeedCurrent;
        float length = Mathf.Abs(targetSpeed - startSpeed) / airspeedAccel;
        float interpolater = 0f;
        while (interpolater < 1f)
        {
            airspeedCurrent = Mathf.Lerp(startSpeed, targetSpeed, interpolater);
            interpolater += (1 / length) * Time.deltaTime;
            yield return null;
        }
        airspeedCurrent = Mathf.Round(airspeedCurrent);
    }

    IEnumerator ChangeRotation()
    {
        Quaternion targetRotation = Quaternion.LookRotation(nextDestination.transform.position - transform.position);
        //StartCoroutine("Roll", targetRotation);

        float angleDirection = Mathf.Sign(Vector3.SignedAngle(transform.forward, nextDestination.transform.position - transform.position, Vector3.up));
        //positive means right turn

        while (transform.rotation != targetRotation)
        {
            targetRotation = Quaternion.LookRotation(nextDestination.transform.position - transform.position) * Quaternion.Euler(0, 0, -rollMaxAngle * angleDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Roll(Quaternion targetRotation)
    {
        float angleDirection = Mathf.Sign(Vector3.SignedAngle(transform.forward, nextDestination.transform.position - transform.position, Vector3.up));
        //positive means right turn
        float interpolater = 0f;
        while (transform.rotation != targetRotation)
        {
            while (Mathf.Abs(planeBody.transform.rotation.z) < rollMaxAngle && transform.rotation != targetRotation)
            {
                planeBody.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0f, -rollMaxAngle * angleDirection, interpolater));
                interpolater += rollSpeedAccel / rollMaxAngle * Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
        while (interpolater > 0)
        {
            planeBody.transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0f, -rollMaxAngle * angleDirection, interpolater));
            interpolater -= rollSpeedAccel / rollMaxAngle * Time.deltaTime;
        }
    }
}
