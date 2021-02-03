using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsCollisionScript : MonoBehaviour
{

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            gm.wayPointCount++;
            Destroy(this.gameObject);
        }
    }
}
