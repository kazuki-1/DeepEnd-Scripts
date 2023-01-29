using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DestroyerStates;
public class DestroyerStateMachine : StateMachineBase
{
    [HideInInspector]
    public DeepEndEnemyController controller;

    public enum StateEnum
    {
        Approach,
        PursuitAndAttack,
        Ramming,
        Sink,
        Retreat, 
        Debug
    }

    public override void Initialize()
    {

        base.Initialize();

        states.Add((int)StateEnum.Approach, new DestroyerState_Approach());
        states.Add((int)StateEnum.PursuitAndAttack, new DestroyerState_PursuitAndAttack());
        //states.Add((int)StateEnum.Ramming, new DestroyerState_Ramming());
        states.Add((int)StateEnum.Sink, new DestroyerState_Sink());
        states.Add((int)StateEnum.Retreat, new DestroyerState_Retreat());
        states.Add((int)StateEnum.Debug, new DestroyerState_Debug());

        foreach (var state in states)
            (state.Value as DestroyerState_Base).controller = parent.GetComponent<DeepEndEnemyController>();
        // Initialization is done when transitioning to state
        //foreach (var state in states)
            //state.Value.Initialize(parent);

    }


}
