using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool timerActive = false;
    public float currentTime;
    public int startMinutes;
    public string currentTimeText;

    void Start()
    {
        currentTime = 0;
    }
    void Update()
    {
        if (timerActive)
        {
            currentTime += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (gameObject.CompareTag("Start"))
            {
                if (!timerActive)
                {
                    StartTimer();
                    Debug.Log(currentTime);
                }
                else
                {
                    Debug.Log(currentTime);
                    StopTimer();
                }
            }

        }
    }

    public void StartTimer()
    {
        timerActive = true;
        Debug.Log("active el timer");
    }
    public void StopTimer()
    {;
        timerActive = false;
        Debug.Log("desactive el timer");
    }
}
