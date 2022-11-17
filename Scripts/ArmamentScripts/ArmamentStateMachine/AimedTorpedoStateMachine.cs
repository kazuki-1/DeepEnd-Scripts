using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AimedTorpedoStates;
public class AimedTorpedoStateMachine : StateMachineBase
{
    [HideInInspector]
    public int reload_time;

    [HideInInspector]
    public AimedTorpedoTubeController controller;
    public enum StateEnum
    {
        Ready, 
        Fire,
        Reload
    }

    // Start is called before the first frame update
    public override void Initialize()
    {

        states.Add((int)StateEnum.Ready,    new AimedTorpedoState_Ready()    );
        states.Add((int)StateEnum.Fire,     new AimedTorpedoState_Fire()     );   
        states.Add((int)StateEnum.Reload,   new AimedTorpedoState_Reload()   );

        foreach (var state in states)
        {
            (state.Value as AimedTorpedState_Base).controller = controller;
        }
        Transition((int)StateEnum.Ready);
        //state.Value.Initialize(parent);
    }

}
