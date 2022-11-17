using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubmarineStates;



public class SubmarineStateMachine : StateMachineBase
{
    public enum StateEnum
    {
        Approach,
        Surface,
        Submerge,
        Attack,
        Sink,
        Retreat
    }

    public override void Initialize()
    {
        base.Initialize();

        states.Add((int)StateEnum.Approach, new SubmarineState_Approach());
        states.Add((int)StateEnum.Surface, new SubmarineState_Surface());
        states.Add((int)StateEnum.Submerge, new SubmarineState_Submerge());
        states.Add((int)StateEnum.Attack, new SubmarineState_Attack());
        states.Add((int)StateEnum.Sink, new SubmarineState_Sunk());
        states.Add((int)StateEnum.Retreat, new SubmarineState_Retreat());



    }
}