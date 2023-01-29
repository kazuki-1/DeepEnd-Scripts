using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using CannonStates;
public class CannonStateMachine : StateMachineBase
{
    [HideInInspector]
    public float reload_time;

    [HideInInspector]
    public CannonController controller;
    public enum StateEnum
    {
        Ready, 
        Fire, 
        Reload
    }

    // Start is called before the first frame update
    public override void Initialize()
    {

        states.Add((int)StateEnum.Ready,    new CannonState_Ready()     );
        states.Add((int)StateEnum.Fire,     new CannonState_Fire()      );
        states.Add((int)StateEnum.Reload,   new CannonState_Reload()    );

        foreach (var state in states)
        {
            //state.Value.Initialize(parent);
            (state.Value as CannonState_Base).controller = controller;
        }
        reload_time = parent.GetComponent<CannonController>().reload_time;
        Transition((int)StateEnum.Ready);

    }

    public override void Initialize(GameObject obj)
    {
        base.Initialize(obj);
    }

}
