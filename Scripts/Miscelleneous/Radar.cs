using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    bool isActivated = false;
    Timer timer;
    static public Radar Get()
    {
        return GameObject.Find("MiniMapViewport").GetComponent<Radar>();

    }

    Camera minimapCamera;
    // Start is called before the first frame update
    void Start()
    {
        minimapCamera = GetComponent<Camera>();
        timer = new Timer(MainController.Get().radarEffectTime);
    }

    // Update is called once per frame
    void Update()
    {
        int mask = LayerMask.NameToLayer("EnemyMinimapObjects");
        if (isActivated)
        {
            minimapCamera.cullingMask |= 1 << mask;
        }
        else
            minimapCamera.cullingMask &= ~(1 << mask);

        if(isActivated)
        { 
            timer.Execute();
            if (timer.Done())
                isActivated = false;
        }

    }
    public void Activate()
    {
        isActivated = true;
        timer.Reset();
    }
    public void Deactivate()
    {
        isActivated = false;
    }
}
