using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLidar : MonoBehaviour {

    //public GameObject trianglePrefab;
    public ObjectPool pool;


    public GameObject test1;
    public float x, y, w, h;

    public List<LidarArcData> lidarData = new List<LidarArcData>();

    void Start () {


        x = GetComponent<RectTransform>().position.x;
        y = GetComponent<RectTransform>().position.y;
        w = GetComponent<RectTransform>().rect.width;
        h = GetComponent<RectTransform>().rect.height;

        /*
        public GameObject[] test;
        /////////
        test = new GameObject[50];
        for(int i=0; i<=49; i++)
        {
            test[i] = GetArc();
            test[i].transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            test[i].transform.position = new Vector3(x, y, 0);
            //RemoveArc(test[i]);
        }
        */

    }
	
	void Update () {
    }


    private int currentStartAngle = 0;
    private int tmpAngle;
    private float tmpRadius;
    private int lengthOfDataAtTimeOfNewAdd;
    private LidarArcData tmpData; // TODO: maybe pool this if GC becomes an issue

    public void DrawNewArc(int radius, int sweepAngle)
    {
        // Get arc from pool
        GameObject arc = GetArc();

        // Make new data contaner object and save all of the arc's properties to it
        tmpData = new LidarArcData();

        tmpData.arcPrefab = arc;
        tmpData.radius = radius;
        tmpData.sweepAngle = sweepAngle;

        tmpAngle = currentStartAngle + sweepAngle;
        while (tmpAngle >= 360)
            tmpAngle -= 360;
        tmpData.startAngle = tmpAngle;

        lengthOfDataAtTimeOfNewAdd = lidarData.Count;
        lidarData.Add(tmpData);



        // Give the arc prefab it's properties
        //arc.transform.rotation = Quaternion.Euler(0, 0, currentStartAngle);
        arc.transform.position = new Vector3(x, y, 0);
        arc.GetComponent<Draw_CircularArc>().DeltaStart = currentStartAngle;
        arc.GetComponent<Draw_CircularArc>().DeltaSize = sweepAngle;

        tmpRadius = (414f * radius / 400f); //426 pixels for full length, 0-400cm range
        if (tmpRadius > 414)
            tmpRadius = 414;

        arc.GetComponent<Draw_CircularArc>().OuterRadius = tmpRadius;
        arc.GetComponent<Draw_CircularArc>().UpdateMesh();


        // Check to see if this arc is overlaping any of the others, and if so return them to the pool
        for (int i = 0; i < lidarData.Count; i++)
        {
            if (i != lengthOfDataAtTimeOfNewAdd) {
                int oldStartAngle = lidarData[i].startAngle;
                int oldEndAngle = oldStartAngle + lidarData[i].sweepAngle;
                int newStartAngle = currentStartAngle;
                int newEndAngle = newStartAngle + sweepAngle;

                while (oldEndAngle >= 360)
                    oldEndAngle -= 360;

                while (newEndAngle >= 360)
                    newEndAngle -= 360;

                // if new end betwwen old bounds or entirely past
                //if ( ((newEndAngle >= oldStartAngle) && (newEndAngle <= oldEndAngle)) || (newStartAngle>oldEndAngle) )
                if ( ((newEndAngle > oldStartAngle) && (newEndAngle < oldEndAngle)))
                {
                    RemoveArc(lidarData[i].arcPrefab);
                    lidarData[i].prefabNeedsReturned = true;
                }
            }

        }
        //*
        for (int i = 0; i < lidarData.Count; i++)
        {
            if (lidarData[i].prefabNeedsReturned)
            {
                lidarData.RemoveAt(i);
            }
        }
        //*/


        currentStartAngle += sweepAngle;
        while (currentStartAngle >= 360)
            currentStartAngle -= 360;


    }

    private GameObject GetArc()
    {
        return pool.GetObjectForType("LidarArc", true);
    }

    private void RemoveArc(GameObject arc)
    {
        pool.PoolObject(arc);
    }
}
