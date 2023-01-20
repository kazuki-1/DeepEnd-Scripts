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
        PursuitAndAttack,
        Sink,
        Retreat
    }

    public override void Initialize()
    {
        base.Initialize();

        states.Add((int)StateEnum.Approach, new SubmarineState_Approach());
        states.Add((int)StateEnum.Surface, new SubmarineState_Surface());
        states.Add((int)StateEnum.Submerge, new SubmarineState_Submerge());
        states.Add((int)StateEnum.PursuitAndAttack, new SubmarineState_PursuitAndAttack());
        states.Add((int)StateEnum.Sink, new SubmarineState_Sunk());
        states.Add((int)StateEnum.Retreat, new SubmarineState_Retreat());

        foreach (var state in states)
            (state.Value as SubmarineState_Base).controller = parent.GetComponent<EnemySubmarineController>();

    }
}