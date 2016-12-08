using UnityEngine;
using System.Collections;

public class LidarArcData : MonoBehaviour {

    // radius is in centimeters
    // angles are in degrees
    public int radius, sweepAngle, startAngle, drawLidarListIndex, age;

    private void Update()
    {
        age--;
    }
}
