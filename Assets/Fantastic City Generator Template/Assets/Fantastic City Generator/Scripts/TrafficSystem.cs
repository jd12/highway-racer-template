using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrafficSystem : MonoBehaviour {


    public GameObject[] IaCars;

    void Awake()
    {
        if (GameObject.Find("RoadMark") && GameObject.Find("RoadMarkRev"))
            InverseCarDirection(true);

        LoadCars();
    }

    private void InverseCarDirection(bool actualside)
    {


        GameObject[] roadMark = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("Road-Mark")).ToArray();
        for (int i = 0; i < roadMark.Length; i++)
            roadMark[i].transform.Find("RoadMark").gameObject.SetActive(actualside);

        roadMark = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("Road-Mark-Rev")).ToArray();
        for (int i = 0; i < roadMark.Length; i++)
            roadMark[i].transform.Find("RoadMarkRev").gameObject.SetActive(!actualside);


    }

    public void LoadCars()
    {

        
        int nVehicles = 0;

        DestroyImmediate(GameObject.Find("CarContainer"));

        Transform CarContainer = new GameObject("CarContainer").transform;
        
        GameObject carro;

        GameObject[] tf01 = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("TF-01")).ToArray();

        int n = tf01.Length;
        for (int i = 0; i < n; i++)
        {

            carro = (GameObject)Instantiate(IaCars[Mathf.Clamp(Random.Range(0, IaCars.Length), 0, IaCars.Length - 1)], tf01[i].transform.position, tf01[i].transform.rotation);
            carro.transform.SetParent(CarContainer);

            carro.GetComponent<TrafficCar>().path = tf01[i].transform.parent.gameObject;

            nVehicles++;

        }

        Debug.Log(nVehicles + " vehicles were instantiated");

    }


}
