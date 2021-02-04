using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsCollisionScript : MonoBehaviour
{

    GameManager gm;
    RoadScript rs;
    CanvasScript checkPointTxtManager;

    public bool carControllerFound = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rs = GameObject.Find("Road").GetComponent<RoadScript>();
        checkPointTxtManager = GameObject.Find("Canvas").GetComponent<CanvasScript>();

        checkPointTxtManager.checkpointTxt.text = "Checkpoints: " + gm.wayPointCount + " / " + gm.waypointReference.Length;


        for (int counter = gm.wayPointCount; counter < gm.waypointReference.Length; counter++)
        {
            gm.waypointReference[counter].SetActive(false); 
            gm.waypointReference[gm.wayPointCount].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
         
        if (other.gameObject.CompareTag("Player"))
        {
            checkPointTxtManager.checkpointTxt.text = "Checkpoints: " + gm.wayPointCount + " / " + gm.waypointReference.Length;
            gm.wayPointCount++;
            if (gm.wayPointCount < gm.waypointReference.Length) 
            {
                gm.waypointReference[gm.wayPointCount].gameObject.SetActive(true);
            }
            Destroy(this.gameObject);
            
        }
        
    }
}
