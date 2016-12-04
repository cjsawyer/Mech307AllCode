using UnityEngine;
using System.Collections;

public class LidarArcData : MonoBehaviour {

    // radius is in centimeters
    // angles are in degrees
    public int radius, sweepAngle, startAngle;
    public GameObject arcPrefab;
    public bool prefabNeedsReturned = false;

}
