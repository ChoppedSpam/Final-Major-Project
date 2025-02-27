using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    public float bpm;
    public AudioSource audiosource;
    public Intervals[] intervals;

    public void Update()
    {
        foreach(Intervals interval in intervals)
        {
            float sampledTime = (audiosource.timeSamples / (audiosource.clip.frequency * interval.GetIntervalLength(bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }
}
 
public class Intervals
{
    public float steps;
    public UnityEvent trigger;
    public int lastInterval;

    public float GetIntervalLength(float _bpm)
    {
        return 60f / (_bpm * steps);
    }

    public void CheckForNewInterval (float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval) 
        {
            lastInterval = Mathf.FloorToInt(interval);
            trigger.Invoke();
        }
    }
}
