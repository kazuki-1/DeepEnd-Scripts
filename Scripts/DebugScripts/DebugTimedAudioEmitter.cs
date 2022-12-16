using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAudioEmitter : MonoBehaviour
{

    public AK.Wwise.Event soundEvent;
    public bool triggerOnClick;
    public bool triggerOnTimer;

    public float timerInterval;
    Stopwatch timer = new Stopwatch();
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0) && triggerOnClick)
            soundEvent.Post(gameObject);


        timer.Execute();
        if (soundEvent != null)
            if (timer.OnPass(timerInterval) && triggerOnTimer)
            {
                soundEvent.Post(gameObject);
            }

    }
}
