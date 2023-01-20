using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : ArmamentBase
{

    // Start is called before the first frame update
    void Awake()
    {

        
        stateMachine = new CannonStateMachine();
        (stateMachine as CannonStateMachine).controller = this;
        stateMachine.Initialize(gameObject);
        
        // Checks the gameObject if there is a ArmamentController
        ArmamentController armamentController = gameObject.GetComponentInParent<ArmamentController>();
        if (armamentController != null)
            reload_time = (int)armamentController.reloadTimes.cannon;
        (stateMachine as CannonStateMachine).reload_time = reload_time;

        starting_position = transform.position;
        starting_direction = transform.forward;
        fireEvent = MainController.Get().batteryParameters.fireSFXEvent;
    }

    // Update is called once per frame
    // public override void Update()
    // {
    //     base.Update();
    // 
    // 
    // }

    public override void Fire()
    {

        if (!isReloading && !outOfBounds)
        {
            stateMachine.Transition((int)CannonStateMachine.StateEnum.Fire);
            fireEvent.Post(gameObject);
        }


    }

    public override float GetRemainingReloadTime()
    {
        CannonStateMachine.StateEnum state = (CannonStateMachine.StateEnum)stateMachine.GetStateEnum();
        if (state == CannonStateMachine.StateEnum.Reload)
            return (stateMachine.curState_ as CannonStates.CannonState_Reload).timer.GetRemainingTime();
        return 0.0f;

    }

    public override List<Vector3> GetTargetDirections()
    {
        List<Vector3> result = new List<Vector3>();

        Vector3 origin = transform.position;
        //Debug.Log(origin);
        origin.y -= 10.0f;
        Vector3 originLeft = origin - transform.right * 10.0f;
        Vector3 originRight = origin + transform.right * 10.0f;

        Vector3 endLeft = originLeft + transform.forward * 1000.0f;
        Vector3 endRight = originRight + transform.forward * 1000.0f;


        result.Add(originLeft);
        result.Add(originRight);
        result.Add(endLeft);
        result.Add(endRight);


        return result;
    }

    public override void Reload()
    {
        stateMachine.Transition((int)CannonStateMachine.StateEnum.Ready);
    }


}
