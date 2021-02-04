using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CanvasScript : MonoBehaviour
{
    [SerializeField] public TMP_Text checkpointTxt;
    [SerializeField] public TMP_Text start;

    UnityStandardAssets.Vehicles.Car.CarUserControl cs;

    public float timerMinutes;
    public float timerSeconds;

    public bool timerFinished = false;
    public bool carControllerFound = false;


    // Start is called before the first frame update
    void Start()
    {
        

        timerMinutes = 3;
        timerSeconds = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!carControllerFound) 
        {
            cs = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>();
            carControllerFound = true;
        }

        startTimer();
        if (timerMinutes == 0)
        {
            start.gameObject.SetActive(false);
            checkpointTxt.gameObject.SetActive(true);
        }
    }


    void startTimer() 
    {
        if (timerMinutes != 0 && timerFinished != false)
        {
            cs.enabled = false;
            start.text = timerMinutes.ToString();

            timerSeconds -= Time.deltaTime;

            if (timerSeconds <= 0)
            {
                timerSeconds = 1;
                timerMinutes--;
            }
        }
        else 
        {
            timerFinished = true;
            cs.enabled = true;
        }
    }
}
