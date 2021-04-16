using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    private float sec;
    private int min;
    private int hou;

    private bool ticking;

    public void UpdateTimer()
    {
        //Count time only whent this is true
        if (ticking == true)
        {

            //Adding seconds
            sec += Time.deltaTime;
            //Adding minutes
            if (Mathf.Floor(sec) >= 60) { sec = 0; min = min + 1; }
            //Adding hours
            if (min >= 60) { min = 0; hou = hou + 1; }
        }
    }
    private void ResetTimer()
    {
        sec = 0;
        min = 0;
        hou = 0;
    }
    public void StartTimer()
    {
        ResetTimer();
        ticking = true;
    }
    public void Stop()
    {
        ResetTimer();
        ticking = false;
    }
    public bool IsTicking()
    {
        return ticking;
    }
    public float getPassedSeconds()
    {
        return sec;
    }
    public int getPassedMinutes()
    {
        return min;
    }
    public int getPassedHours()
    {
        return hou;
    }
}
