using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class RoadController : MonoBehaviour
{

    [Header("Disabler")]
    [SerializeField] public bool removeTrack = false;

    [Header("Enabler")]
    [SerializeField] public bool genTrack = false;


    GameObject raceTrack, road;

    // Start is called before the first frame update
    /*void Start()
    {
        GenerateTrack();
    }*/


    private void OnValidate()
    {
        if (genTrack)
        {
            GenerateTrack();
            genTrack = false;
        }
    }


    void GenerateTrack() 
    {

        raceTrack = new GameObject("Race Track");
        road = new GameObject("Road");
        road.transform.position = Vector3.zero;
        road.AddComponent<RoadScript>();
        road.GetComponent<RoadScript>().GenerateRoad();
        road.GetComponent<RoadScript>().AddMaterials();
        road.transform.SetParent(raceTrack.transform);

    }

}
