using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestroyerStates;
public class DestroyerStateMachine : StateMachineBase
{


    public enum StateEnum
    {
        Approach,
        PursuitAndAttack,
        Ramming,
        Sink, 
        Debug
    }

    public override void Initialize()
    {

        base.Initialize();

        states.Add((int)StateEnum.Approach, new DestroyerState_Approach());
        states.Add((int)StateEnum.PursuitAndAttack, new DestroyerState_PursuitAndAttack());
        states.Add((int)StateEnum.Ramming, new DestroyerState_Ramming());
        states.Add((int)StateEnum.Sink, new DestroyerState_Sunk());
        states.Add((int)StateEnum.Debug, new DestroyerState_Debug());
        // Initialization is done when transitioning to state
        //foreach (var state in states)
            //state.Value.Initialize(parent);

    }


}
