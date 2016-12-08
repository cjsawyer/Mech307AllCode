using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLidar : MonoBehaviour {

    //public GameObject trianglePrefab;
    public ObjectPool pool;


    public GameObject test1;
    public float x, y, w, h;

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

    private List<GameObject> nonPooledPrefabs = new List<GameObject>();
    private GameObject tmpGameObject;
    private LidarArcData tmpLidarArcData;
    private int tmpAge = 0;

    private int currentStartAngle = 0;
    private int tmpAngle;
    private int tmpRadius;
    private int lengthOfDataAtTimeOfNewAdd;

    public void DrawNewArc(int radius, int sweepAngle)
    {
        // Get arc from pool
        GameObject arc = GetArc();
        nonPooledPrefabs.Add(arc);
        tmpAge = 500;

        tmpAngle = currentStartAngle + sweepAngle;
        while (tmpAngle >= 360)
            tmpAngle -= 360;


        // Give the arc prefab it's properties
        //arc.transform.rotation = Quaternion.Euler(0, 0, currentStartAngle);
        arc.transform.position = new Vector3(x, y, 0);
        arc.GetComponent<Draw_CircularArc>().DeltaStart = currentStartAngle;
        arc.GetComponent<Draw_CircularArc>().DeltaSize = sweepAngle;

        tmpRadius = (414 * radius / 400); //426 pixels for full length, 0-400cm range
        if (tmpRadius > 414)
            tmpRadius = 414;

        arc.GetComponent<Draw_CircularArc>().OuterRadius = tmpRadius;
        arc.GetComponent<Draw_CircularArc>().UpdateMesh();




        // Store the prefab's geometry data
        arc.GetComponent<LidarArcData>().startAngle = currentStartAngle;
        arc.GetComponent<LidarArcData>().sweepAngle = sweepAngle;
        arc.GetComponent<LidarArcData>().radius = tmpRadius;
        arc.GetComponent<LidarArcData>().age = tmpAge;
        arc.GetComponent<LidarArcData>().drawLidarListIndex = nonPooledPrefabs.Count;




        // Check to see if this arc is overlaping any of the others, and if so return them to the pool

        for ( int i=0; i< nonPooledPrefabs.Count; i++)
        {



            tmpGameObject = nonPooledPrefabs[i];
            if (tmpGameObject != null)
            {
                tmpLidarArcData = tmpGameObject.GetComponent<LidarArcData>();
                float otherStartAngle = tmpLidarArcData.startAngle;
                float otherEndAngle = otherStartAngle + tmpLidarArcData.sweepAngle;

                int newStartAngle = currentStartAngle;
                int newEndAngle = newStartAngle + sweepAngle;

                //*
                while (otherEndAngle >= 360)
                    otherEndAngle -= 360;

                while (newEndAngle >= 360)
                    newEndAngle -= 360;
                //*/

                // if new end betwwen old bounds or entirely past
                //if ( ((newEndAngle >= oldStartAngle) && (newEndAngle <= oldEndAngle)) || (newStartAngle<oldEndAngle) )
                //if (oldEndAngle > 180)
                // if ( (newEndAngle > oldStartAngle) && (newEndAngle < oldEndAngle)  && (tmpLidarArcData.drawLidarListIndex < nonPooledPrefabs.Count) )
                //if ( (newStartAngle < oldEndAngle)  && (tmpLidarArcData.drawLidarListIndex < i) )
                //if ((newEndAngle > oldStartAngle) && (newEndAngle < oldEndAngle))
                Debug.Log("newEndAngle: " + newEndAngle);
                Debug.Log("otherStartAngle: " + otherStartAngle);
                Debug.Log("tmpLidarArcData.age: " + tmpAge);
                Debug.Log("i: " + i);
                Debug.Log(" nonPooledPrefabs[i].GetComponent<LidarArcData>().age: " + nonPooledPrefabs[i].GetComponent<LidarArcData>().age);
                Debug.Log("------------------------------");

                // if ((newEndAngle > otherStartAngle) && (tmpAge < nonPooledPrefabs[i].GetComponent<LidarArcData>().age))
                if (
                    (
                        ((newStartAngle >= otherEndAngle) && (newStartAngle < otherEndAngle))
                        ||
                        ((newEndAngle >= otherStartAngle) && (newEndAngle < otherEndAngle))
                    )
                )
                {
                    RemoveArc(nonPooledPrefabs[i]);
                    nonPooledPrefabs.RemoveAt(i--);
                }


                // This handles the transition between 360->0 degrees
                else if (newEndAngle < newStartAngle)
                {
                    //RemoveArc(nonPooledPrefabs[i]);
                    //nonPooledPrefabs.RemoveAt(i--);
                }

            }
        }


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
