using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    bool isActivated = false;
    Timer timer;

    [SerializeField]
    AK.Wwise.Event playSonarSound;

    [SerializeField]
    AK.Wwise.Event stopSonarSound;

    GameObject player;
    static public Sonar Get()
    {
        return GameObject.Find("MiniMapViewport").GetComponent<Sonar>();

    }

    Camera minimapCamera;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
                Deactivate();
        }

    }
    public void Activate()
    {
        playSonarSound.Post(player);
        isActivated = true;
        timer.Reset();
    }
    public void Deactivate()
    {
        stopSonarSound.Post(player);
        isActivated = false;
    }
}
