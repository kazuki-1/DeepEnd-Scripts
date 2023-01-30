using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    bool isActivated = false;
    Timer effectTimer;
    Timer cooldownTimer;

    [SerializeField]
    AK.Wwise.Event playSonarSound;

    [SerializeField]
    AK.Wwise.Event stopSonarSound;

    [SerializeField]
    float radarEffectTime = 10.0f;

    [SerializeField]
    float cooldownTime = 15.0f;

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
        effectTimer = new Timer(radarEffectTime);
        cooldownTimer = new Timer(cooldownTime);
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
            effectTimer.Execute();
            if (effectTimer.Done())
                Deactivate();
        }

        cooldownTimer.Execute();

    }
    public void Activate()
    {
        if (!cooldownTimer.Done())
            return;
        playSonarSound.Post(player);
        isActivated = true;
        effectTimer.Reset();
    }
    public void Deactivate()
    {
        stopSonarSound.Post(player);
        isActivated = false;
    }
}
