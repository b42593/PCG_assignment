using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoadController : MonoBehaviour
{
    GameObject raceTrack, road;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrack();
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
