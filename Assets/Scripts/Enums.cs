using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaypointPathType
{
    Entry,
    Holding,
    Approach,
    Landing,
    TaxiArrival,
    TaxiDeparture,
    Takeoff,
    Exit
}

public enum Jurisdiction
{
    Handoff,
    Tower,
    Ground,
    Terminal,
    Departure
}