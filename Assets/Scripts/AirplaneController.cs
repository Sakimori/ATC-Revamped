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

    [SerializeField]
    Jurisdiction jurisdiction = Jurisdiction.Handoff;
    
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
    float rollResetSpeed;

    [SerializeField]
    float turnSpeed;

    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Flight;
        transform.LookAt(nextDestination.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Flight)
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
            ChangeJurisdiction(thisWaypoint.jurisdiction);
            if (!thisWaypoint.isTerminal)
            {
                if (!(thisWaypoint.forcedSpeed == 0))
                {
                    StopCoroutine("ChangeSpeed");
                    StartCoroutine("ChangeSpeed", thisWaypoint.forcedSpeed);
                }
                nextDestination = thisWaypoint.nextWaypoint.GetComponent<Waypoint>().gameObject;
                if (state == State.Flight)
                {
                    StopCoroutine("ChangeRotationInFlight");
                    StartCoroutine("ChangeRotationInFlight");
                }

            }
            else
            {
                state = State.Parked;
                nextDestination = gameObject;
            }
        }
    }

    public void NewWaypoint(Waypoint waypoint)
    {
        StopCoroutine("ChangeSpeed");
        StopCoroutine("ChangeRotationInFlight");
        ChangeJurisdiction(waypoint.jurisdiction);
        nextDestination = waypoint.gameObject;
        if (!(waypoint.forcedSpeed == 0))
        {
            StartCoroutine("ChangeSpeed", waypoint.forcedSpeed);
        }
        StartCoroutine("ChangeRotationInFlight");
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

    IEnumerator ChangeRotationInFlight()
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

        while (Mathf.Abs(transform.rotation.eulerAngles.z) > 2)
        {
            targetRotation = Quaternion.LookRotation(transform.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rollResetSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void ChangeJurisdiction(Jurisdiction newJurisdiction)
    {
        jurisdiction = newJurisdiction;
        switch (newJurisdiction)
        {
            case Jurisdiction.Handoff:
                state = State.Flight;
                break;
            case Jurisdiction.Tower:
                Waypoint thisWaypoint = nextDestination.GetComponent<Waypoint>();
                switch (thisWaypoint.type)
                {
                    case WaypointPathType.Landing:
                        state = State.Landing;
                        break;
                    case WaypointPathType.Takeoff:
                        state = State.Takeoff;
                        break;
                    default:
                        break;
                }
                break;
            case Jurisdiction.Ground:
                state = State.Taxi;
                break;
            case Jurisdiction.Terminal:
                state = State.Parked;
                break;
            default:
                break;
        }
    }

}
