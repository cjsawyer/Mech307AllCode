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
    private int currentStartAngle = 0;
    private int tmpAngle;
    private float tmpRadius;
    private int lengthOfDataAtTimeOfNewAdd;

    public void DrawNewArc(int radius, int sweepAngle)
    {
        // Get arc from pool
        GameObject arc = GetArc();
        nonPooledPrefabs.Add(arc);

        tmpAngle = currentStartAngle + sweepAngle;
        while (tmpAngle >= 360)
            tmpAngle -= 360;

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

        Debug.Log("" + nonPooledPrefabs.Count);
        for ( int i=0; i< nonPooledPrefabs.Count; i++)
        {



            tmpGameObject = nonPooledPrefabs[i];
            if (tmpGameObject != null)
            {
                float oldStartAngle = tmpGameObject.GetComponent<Draw_CircularArc>().DeltaStart;
                float oldEndAngle = oldStartAngle + tmpGameObject.GetComponent<Draw_CircularArc>().DeltaSize;
                Debug.Log(oldStartAngle + "_" + oldEndAngle);

                int newStartAngle = currentStartAngle;
                int newEndAngle = newStartAngle + sweepAngle;

                /*
                while (oldEndAngle >= 360)
                    oldEndAngle -= 360;

                while (newEndAngle >= 360)
                    newEndAngle -= 360;
                */

                // if new end betwwen old bounds or entirely past
                //if ( ((newEndAngle >= oldStartAngle) && (newEndAngle <= oldEndAngle)) || (newStartAngle>oldEndAngle) )
                //if (oldEndAngle > 180)
                if ( ((newEndAngle > oldStartAngle) && (newEndAngle < oldEndAngle)))
                {
                    RemoveArc(nonPooledPrefabs[i]);
                    nonPooledPrefabs.RemoveAt(i--);

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
