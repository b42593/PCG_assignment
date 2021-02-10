using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class RoadController : MonoBehaviour
{
    GameObject raceTrack, road, plane;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTrack();
        GenerateGround();
    }

    void GenerateTrack() 
    {
        //Create Road GameObject as child of racetrack
        raceTrack = new GameObject("Race Track");
        road = new GameObject("Road");
        road.transform.position = Vector3.zero;
        road.AddComponent<RoadScript>();
        road.GetComponent<RoadScript>().GenerateRoad();
        road.GetComponent<RoadScript>().AddMaterials();
        road.transform.SetParent(raceTrack.transform);
    }

    void GenerateGround()
    {
        //Create Ground GameObject as child of racetrack
        plane = new GameObject("Ground");
        plane.transform.position = new Vector3(-1102f, -0.5f, -1078f);
        plane.tag = "Ground";
        plane.AddComponent<GroundScript>();
        plane.GetComponent<GroundScript>().cellSize = 100;
        plane.GetComponent<GroundScript>().width = 22;
        plane.GetComponent<GroundScript>().height = 22;
        plane.GetComponent<GroundScript>().GeneratePlane();

        if (SceneManager.GetActiveScene().name == "Level1" )
        {
            plane.GetComponent<GroundScript>().AddMaterials(0);
        }
        if (SceneManager.GetActiveScene().name == "Level2") 
        {
            plane.GetComponent<GroundScript>().AddMaterials(1);
        }
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            plane.GetComponent<GroundScript>().AddMaterials(2);
        }

        plane.transform.SetParent(raceTrack.transform);
    }

}
