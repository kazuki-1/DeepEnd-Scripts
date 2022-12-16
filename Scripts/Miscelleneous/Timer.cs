using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Timer class, call execute each frame 
public class Timer 
{

    public float time;     // Seconds
    private float remaining_time;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tm">How long you want the timer to be in seconds</param>
    public Timer(int tm) 
    {
        time = tm;
        remaining_time = time;
    }

    public void Execute()
    {
        
        remaining_time -= Time.deltaTime;
        remaining_time = Mathf.Max(remaining_time, 0.0f);
    }

    /// <summary>
    /// Returns the value the timer is set to be
    /// </summary>
    /// <returns></returns>
    public float GetTime()
    {
        return time;
    }
    public float GetRemainingTime()
    {
        return remaining_time;
    }
    public void Reset()
    {
        remaining_time = time;
    }

    // returns true if timer is done
    public bool Done()
    {
        return remaining_time == 0.0f;
    }

    public bool OnPass(float tm)
    {
        return time - remaining_time > tm;
    }

}


public class Stopwatch
{
    float time = 0.0f;
    private float timerInterval;

    /// <summary>
    /// Call every frame to update time
    /// </summary>
    public void Execute()
    {
        time += Time.deltaTime;
    }

    /// <summary>
    /// Returns true everytime stopwatch passed tm
    /// </summary>
    /// <returns></returns>
    public bool OnPass(float tm)
    {
        if(timerInterval == 0.0f)
            timerInterval = tm;

        bool result = time > tm ;
        if (result)
            timerInterval += tm;

        return result;
    }
}