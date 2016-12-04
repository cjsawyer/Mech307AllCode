using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateUiElements : MonoBehaviour {

    public ParseStream stream;
    public Text textSpeed, lightsSwitch, throttleSwitch;
    public RectTransform throttleBar;
    public DrawLidar lidar;


    // BIKE CONSTANTS
    // speed [mi/hr] = rotations*sec circumfrence/sec  *  ft/inch * mi/ft  * sec/min * min/hr   =   2*pi*r * 1/12*1/5280 * 60*60 
    float BIKE_TIRE_RADIUS = 12; //in inches TODO: CHECK THIS


    string[] LIGHT_MODES = { "ON", "OFF", "AUTO"};
    string[] THROTTLE_MODES = { "THROTTLE", "OFF", "ASSIST" };

    void Start()
    {
    }

    void Update()
    {
    }

    public void UpdateUI()
    {

        while(stream.lidarRadius.Count > 0)
        {
            lidar.DrawNewArc(stream.lidarRadius[0], stream.lidarTheta[0]);
            stream.lidarRadius.RemoveAt(0);
            stream.lidarTheta.RemoveAt(0);
        }


        float speed = stream.cadence / (stream.cadenceTime/1000f) * (2 * Mathf.PI * BIKE_TIRE_RADIUS * 1 / 12f * 1 / 5280f) * (60 * 60);
        textSpeed.text = string.Format("{0:0.00}", speed);

        float throttlePercent = (stream.throttle-46) / (169f); // normlalize to Throttle's weird voltage range. (min:47, max:215)
                                                               // normalize new range from 1 to (215-47+1)=169, with the 1 to keep it visible at 0
        throttleBar.anchorMax = new Vector2(throttlePercent, 1);

        lightsSwitch.text = LIGHT_MODES[stream.lightsSwitchPosition];
        throttleSwitch.text = THROTTLE_MODES[stream.throttleSwitchPosition];
    }




}
