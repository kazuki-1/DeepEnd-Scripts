using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Timer class, call execute each frame 
public class Timer 
{

    public float time;     // Seconds
    private float remaining_time;
    private int timerPasses = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tm">How long you want the timer to be in seconds</param>
    public Timer(int tm) 
    {
        time = tm;
        //remaining_time = time;
        remaining_time = 0;
    }

    public Timer()
    {
        remaining_time = time = 0.0f;

    }

    public Timer(float tm)
    {
        time = tm;
        //remaining_time = time;
        remaining_time = 0;
    }

    public void Execute()
    {

        remaining_time += Time.deltaTime;
        if(time > 0.0f)
            remaining_time = Mathf.Min(remaining_time, time);
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
        //return remaining_time;
        return time - remaining_time;
    }
    /// <summary>
    /// Resets the timer and set the trigger time to the given parameter
    /// </summary>
    /// <param name="tm"></param>
    public void ResetTo(float tm)
    {
        time = tm;
        remaining_time = 0.0f;
    }
    public void Reset()
    {
        // remaining_time = time;
        remaining_time = 0;
    }

    // returns true if timer is done
    public bool Done()
    {
        //return remaining_time == 0.0f;
        return remaining_time >= time ;
    }

    /// <summary>
    /// Returns true if timer has passed the given time
    /// </summary>
    public bool OnPass(float tm)
    {
        // return time - remaining_time > tm;
        return remaining_time > tm;
    }

    public bool OnPassOnce(float tm)
    {
        float prev_time = remaining_time - Time.deltaTime;
        if (prev_time < tm && remaining_time > tm)
            return true;
        return false;
    }

    /// <summary>
    /// Returns true everytime the timer passes the given time
    /// </summary>
    /// <param name="tm"></param>
    /// <returns></returns>
    public bool OnEveryPass(float tm)
    {
        bool res = false;
        float interval = tm * timerPasses;
        if(remaining_time > interval)
        {
            res = true;
            timerPasses = (int)(remaining_time / tm) + 1;
        }
        return res;

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