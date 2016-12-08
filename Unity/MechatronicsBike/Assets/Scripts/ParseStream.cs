using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

using UnityEngine.UI;

public class ParseStream : MonoBehaviour
{

    /*
    serpator start lidar lidar lidar throttle lights cadence end
    public string incoming = "_-[|16,120|30,100|25,160[100,1[2[45,200+";

    LIDAR, from 2nd Arduino
    [|angleChange,distance|angleChange,distange.....

    throttle, from PIC1
    [throttle,switch
        THROTTLE:   min:47    max:215
        SWITCH:     0   THROTTLE
                    1   OFF
                    2   ASSIST

    lights, from PIC2
    [switch
        SWITCH  0   on
                1   off
                2   auto

    cadence, from Main Arduino
    [wheel revs, time ms since last]
        wheel rotations since last count
                  


    */

    public UpdateUiElements updateUI;

    const string SEPERATOR = "_";
    const string START = "-";
    const string END = "+";
    const string SOURCE_SEPERATR = "["; // between sources/chunks of information
    const string LIDAR_SEPERATOR = "|"; // to differentiate couple pairs in the lidar chunk
    const string INNERSEPERATOR = ","; // to differentiate couple pairs in the lidar chunk


    //serpator start lidar throttle lights cadence end
    public string notepad = "_-[|16,120|30,100|25,160[100,1[2[45,200+";


    //public string incoming = "_-[|16,120|30,100|25,160[100,1[2[45,200+";
    public string incoming = "";
    public string leftOverString = "";

    public List<int> lidarTheta = new List<int>();
    public List<int> lidarRadius = new List<int>();

    public int throttle = 0;
    public int throttleSwitchPosition = 0;
    public int lightsSwitchPosition = 0;
    public int cadence = 0;
    public int cadenceTime = 0;

    public bool needUpdate = false;
    void Start()
    {
    }

    void Update()
    {
        if (needUpdate || (incoming.Length!=0))
        {
            Parse();
            updateUI.UpdateUI();
            needUpdate = false;
        }
    }


    public bool packetHasBothEnds, packetEndsOrder;

    private void Parse()
    {
        if (leftOverString.Length != 0)    // if there was unusable data from end of the last string, add it before the new one
        {
            incoming = leftOverString + incoming;
            leftOverString = "";
        }

        string[] sections = incoming.Split(new string[] { SEPERATOR }, StringSplitOptions.RemoveEmptyEntries); // main packet differentiator -{(,)(,)(,)}[,,]<>+

        foreach (string s in sections)
        {
            // Make sure this substring has both start and end strings, "-" and "+"
            packetHasBothEnds = (s.Contains(START) && s.Contains(END));
            packetEndsOrder = (s.IndexOf(START) < s.IndexOf(END));

            if ((packetHasBothEnds) && (packetEndsOrder))
            {
                int start = s.IndexOf(START);
                int end = s.IndexOf(END);

                string chunk = s.Substring(start + 1, end - start - 1);
                //Debug.Log("chunk: " + chunk);

                //"_-[|16,120|30,100|25,160[100,1[2[45+"
                string[] sources = chunk.Split(new string[] { SOURCE_SEPERATR }, StringSplitOptions.RemoveEmptyEntries);
                //Debug.Log(""+sources.Length);

                //serpator start lidar throttle lights cadence end
                //Debug.Log("lidar: "+sources[0]);
                //Debug.Log("throttle: "+sources[1]);
                //Debug.Log("lights: "+sources[2]);
                //Debug.Log("cadence: "+sources[3]);

                //Debug.Log("---------------");
                string[] lidarData = sources[0].Split(new string[] { LIDAR_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string l in lidarData)
                {
                    //Debug.Log("lidar couple: " + l);
                    int commaIndex = l.IndexOf(INNERSEPERATOR);
                    lidarTheta.Add(int.Parse(l.Remove(commaIndex)));
                    lidarRadius.Add(int.Parse(l.Remove(0, commaIndex + 1)));
                    //Debug.Log("theta: " + theta);
                    //Debug.Log("radius: " + radius);
                    //Debug.Log("------");
                }

                //Debug.Log("---------------");
                int commaIndexThrottle = sources[1].IndexOf(INNERSEPERATOR);

                throttle = int.Parse(sources[1].Remove(commaIndexThrottle));
                throttleSwitchPosition = int.Parse(sources[1].Remove(0, commaIndexThrottle + 1));
                //Debug.Log("throttle: " + throttle);
                //Debug.Log("switchPosition: " + switchPosition);


                //Debug.Log("---------------");
                lightsSwitchPosition = int.Parse(sources[2]);
                //Debug.Log("lights: " + lights);

                //Debug.Log("---------------");
                int commaIndexCadence = sources[3].IndexOf(INNERSEPERATOR);
                cadence = int.Parse(sources[3].Remove(commaIndexCadence));
                cadenceTime = int.Parse(sources[3].Remove(0, commaIndexCadence + 1));
                //Debug.Log("cadence: " + cadence);
                //Debug.Log("cadenceTime: " + cadenceTime);

            }
            else
            {
                if (s.Contains(START) && !s.Contains(END)) // if this is the BEGGINING of an incomplete {} section
                {
                    leftOverString += s; // save it for the next loop
                }
            }

        }

        incoming = "";
    }

}