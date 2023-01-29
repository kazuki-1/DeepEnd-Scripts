using System.Collections;
using System.Collections.Generic;
using HomingTorpedoStates;
using UnityEngine;


public class HomingTorpedoStateMachine : StateMachineBase
{

    [HideInInspector]
    public float reload_time;

    [HideInInspector]
    public HomingTorpedoTubeController controller;
    public enum StateEnum
    {
        Ready, 
        Target, 
        Fire, 
        Reload
    
    }


    // Start is called before the first frame update
    public override void Initialize()
    {
        states.Add((int)StateEnum.Ready,    new HomingTorpedoState_Ready()  );
        states.Add((int)StateEnum.Target,   new HomingTorpedoState_Target() );
        states.Add((int)StateEnum.Fire,     new HomingTorpedoState_Fire()   );
        states.Add((int)StateEnum.Reload,   new HomingTorpedoState_Reload() );

        foreach (var state in states)
            (state.Value as HomingTorpedoState_Base).controller = controller;
        

        reload_time = controller.reload_time;
        Transition((int)StateEnum.Ready);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
