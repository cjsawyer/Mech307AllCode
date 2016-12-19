using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateUiElements : MonoBehaviour {

    public ParseStream stream;
    public Text textSpeed, lightsSwitch, throttleSwitch;
    public RectTransform throttleBar;
    public DrawLidar lidar;
    public AudioSource lightOff, lightOn, lightAuto, motorOff, motorOn, motorAssist;


    // BIKE CONSTANTS
    // speed [mi/hr] = rotations*sec circumfrence/sec  *  ft/inch * mi/ft  * sec/min * min/hr   =   2*pi*r * 1/12*1/5280 * 60*60 
    float BIKE_TIRE_RADIUS = 23; //in inches TODO: CHECK THIS


    string[] LIGHT_MODES = { "ON", "OFF", "AUTO"};
    string[] THROTTLE_MODES = { "THROTTLE", "OFF", "ASSIST" };

    void Start()
    {
    }

    void Update()
    {
    }

    int oldSL = -1, oldST = -1;

    public void UpdateUI()
    {

        while (stream.lidarRadius.Count > 0)
        {
            lidar.DrawNewArc(stream.lidarRadius[0], stream.lidarTheta[0]);
            stream.lidarRadius.RemoveAt(0);
            stream.lidarTheta.RemoveAt(0);
        }


        float speed = stream.cadence / (stream.cadenceTime / 1000f) * (2 * Mathf.PI * BIKE_TIRE_RADIUS * 1 / 12f * 1 / 5280f) * (60 * 60);
        textSpeed.text = string.Format("{0:0.00}", speed);

        float rangeTop = 220;
        float rangeBottom = 40;
        float throttlePercent = (stream.throttle - rangeBottom) / (rangeTop - rangeBottom + 1); // normlalize to Throttle's weird voltage range. (min:47, max:215)
                                                                                                // normalize new range from 1 to (215-47+1)=169, with the 1 to keep it visible at 0
        throttleBar.anchorMax = new Vector2(throttlePercent, 1);

        

        lightsSwitch.text = LIGHT_MODES[stream.lightsSwitchPosition];
        throttleSwitch.text = THROTTLE_MODES[stream.throttleSwitchPosition];


        if (oldSL == -1)
        {
            // First loop, need to set dummy data
            oldSL = stream.lightsSwitchPosition;
            oldST = stream.throttleSwitchPosition;
        }

        //public AudioSource lightOff, lightOn, lightAuto, motorOff, motorOn, motorAssist;

        if (oldSL != stream.lightsSwitchPosition)
        {
            switch (stream.lightsSwitchPosition)
            {
                case 0:
                    lightOn.Play();
                    break;
                case 1:
                    lightOff.Play();
                    break;
                case 2:
                    lightAuto.Play();
                    break;
            }
        }
        if (oldST != stream.throttleSwitchPosition)
        {

            switch (stream.throttleSwitchPosition)
            {
                case 0:
                    motorOn.Play();
                    break;
                case 1:
                    motorOff.Play();
                    break;
                case 2:
                    motorAssist.Play();
                    break;
            }

        }

        oldSL = stream.lightsSwitchPosition;
        oldST = stream.throttleSwitchPosition;

        /*
         * string[] LIGHT_MODES = { "ON", "OFF", "AUTO"};
            string[] THROTTLE_MODES = { "THROTTLE", "OFF", "ASSIST" }
*/

    }



}
