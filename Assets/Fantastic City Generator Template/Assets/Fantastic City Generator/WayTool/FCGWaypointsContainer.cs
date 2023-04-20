
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FCGWaypointsContainer : MonoBehaviour {

    //[HideInInspector]        
    public List<Transform> waypoints = new List<Transform>();

    [HideInInspector]
    public GameObject[] nextWay;

    private GameObject[] tf01;
    
    private bool inLeft = false;
    
    private void Awake()
    {
       
        inLeft = GameObject.Find("RoadMarkRev");
        
        tf01 = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("TF-01")).ToArray();

            
        for (int i = 0; i < waypoints.Count; i++)
        if (i < waypoints.Count - 1)
            waypoints[i].LookAt(waypoints[i + 1]);
        else
        {
            waypoints[i].rotation = Quaternion.LookRotation(waypoints[i].position - waypoints[i - 1].position);
            NextWays(waypoints[i]);
        }
       

       
    }

  
    public void InvertNodesDirection()
    {
        Vector3 wtemp = new Vector3(0,0,0);
         
        int nn = Mathf.CeilToInt(waypoints.Count / 2);
   
        for (int i = 0; i < nn; i++)
        {
            wtemp = waypoints[i].position;
            waypoints[i].position = waypoints[waypoints.Count - i - 1].position;
            waypoints[waypoints.Count - i - 1].position = wtemp;
        }

    }
 

    private void NextWays(Transform referencia)
    {
   
             
        int n = tf01.Length;

        if (n < 1) return;
       
        ArrayList arr = new ArrayList();

        for (int i = 0; i < n; i++)
        {

            float distancia = Vector3.Distance(referencia.position, tf01[i].transform.position);

                
            if (distancia < 35f && distancia > 8f)
            {

                float a = GetAngulo(referencia, tf01[i].transform);

                if (!inLeft && (a > 340 || a < 80) || inLeft && (a > 280 || a < 20)) 
                arr.Add(tf01[i].transform);
                
            }

        }

        referencia.transform.parent.gameObject.GetComponent<FCGWaypointsContainer>().AddNewWays(referencia, arr);

    }


    private void AddNewWays(Transform referencia, ArrayList arr)
    {
        Transform stemp;

        nextWay = new GameObject[arr.Count];

        for (int i = 0; i < arr.Count; i++)
        {

                stemp = (Transform)arr[i];
                nextWay[i] = stemp.parent.gameObject;
            

        }



    }

    private float GetAngulo(Transform origem , Transform target)
    {
        float r;

        GameObject compass = new GameObject("Compass");
        compass.transform.parent = origem;
        compass.transform.localPosition = new Vector3(0, 0, 0);

        compass.transform.LookAt(target);
        r = compass.transform.localEulerAngles.y;

        Destroy(compass);
        return r;

    }

    void OnDrawGizmos() {

        if (waypoints.Count < 1) return;

        for (int i = 0; i < waypoints.Count; i ++){

            Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.3f);

            if (waypoints.Count < 1) return;

            Gizmos.DrawSphere(waypoints[i].transform.position, 1f);
			Gizmos.DrawWireSphere (waypoints[i].transform.position, 2f);

            if (waypoints.Count < 2) return;

            if (i < waypoints.Count - 1){
				if(waypoints[i] && waypoints[i+1]){
                    
					if (waypoints.Count > 0) {

                        if (i < waypoints.Count - 1)
                        {
                            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                            waypoints[i].LookAt(waypoints[i + 1]);
                           
                        } 

					}
				}
			}
            else if (i == waypoints.Count - 1)
            {
                waypoints[i].rotation = waypoints[i - 1].rotation; 
            }

        }
		
	}
 
    
}
