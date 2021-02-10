using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainScript : MonoBehaviour
{

    public int rainChoice;

    [SerializeField] GameObject raindroplets;

    // Start is called before the first frame update
    void Start()
    {

        GameObject rain;

        rainChoice = Random.Range(0, 2);

        if (rainChoice == 1)
        {
            rain = Instantiate(raindroplets, this.transform.position, this.transform.rotation);
            var emission = rain.GetComponentInChildren<ParticleSystem>().emission;
            rain.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            emission.enabled = true;
            rain.name = "rain";
            rain.transform.SetParent(this.transform);
        }
    }
}
