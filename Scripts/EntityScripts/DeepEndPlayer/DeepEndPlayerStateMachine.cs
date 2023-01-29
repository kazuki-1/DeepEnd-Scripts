using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayerStates;

public class DeepEndPlayerStateMachine : StateMachineBase
{
    enum StateEnum
    {
        Alive, 
        Sunk
    }


    public override void Initialize()
    {
        base.Initialize();
        states.Add((int)StateEnum.Alive, new PlayerState_Alive());
        states.Add((int)StateEnum.Sunk, new PlayerState_Sunk());

    }
    // Update is called once per frame
}
