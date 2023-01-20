using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedTorpedoTubeController : ArmamentBase
{
    public Vector2 deadZoneAngle;
    

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new AimedTorpedoStateMachine();
        (stateMachine as AimedTorpedoStateMachine).controller = this;
        stateMachine.Initialize(gameObject);



        ArmamentController armamentController = gameObject.GetComponentInParent<ArmamentController>();
        if (armamentController != null)
            reload_time = armamentController.reloadTimes.aimedTorpedo;
        (stateMachine as AimedTorpedoStateMachine).reload_time = reload_time;

        starting_position = transform.position;
        starting_direction = transform.forward;

    }

    public override void Fire()
    {
        ArmamentController controller = GetComponentInParent<ArmamentController>();
        if ((!outOfBounds && !isReloading ) &&  !controller.CheckAmmo())
        {
            stateMachine.Transition((int)AimedTorpedoStateMachine.StateEnum.Fire);
            controller.munitions.aimedTorpedo -= barrel_count;
        }
    }

    public override void RotateToPoint(Vector3 point)
    {
        Vector3 p = new Vector3(point.x, 0.0f, point.z);                            // Target point
        Vector3 o = new Vector3(starting_direction.x, 0.0f, starting_direction.z);  // Origin

        float angle_diff = Vector3.Angle(p.normalized, o.normalized);
        if (Mathf.Abs(angle_diff) > deadZoneAngle.x && Mathf.Abs(angle_diff) < deadZoneAngle.y)             // Checks if is within deadzone angle
        {
            outOfBounds = true;
            return;
        }
        else
            outOfBounds = false;

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Quaternion q = Quaternion.LookRotation(forward, transform.up);
        target_direction = point.normalized;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, 0.3f);

    }


    // Returns the remaining reload time for the armament, returns 0 if not in reload state
    public override float GetRemainingReloadTime()
    {

        AimedTorpedoStateMachine.StateEnum state = (AimedTorpedoStateMachine.StateEnum)stateMachine.GetStateEnum();
        if (state == AimedTorpedoStateMachine.StateEnum.Reload)
            return (stateMachine.curState_ as AimedTorpedoStates.AimedTorpedoState_Reload).timer.GetRemainingTime();
        return 0.0f;


    }

    public override void OnDrawGizmosSelected()
    {
        Vector3 frontRight, backRight;
        frontRight = transform.eulerAngles;
        frontRight.y += deadZoneAngle.x;

        backRight = transform.eulerAngles;
        backRight.y += deadZoneAngle.y;

        Vector3 frontLeft, backLeft;
        frontLeft = transform.eulerAngles;
        frontLeft.y -= deadZoneAngle.x;

        backLeft = transform.eulerAngles;
        backLeft.y -= deadZoneAngle.y;


        Vector3 f_Right = Quaternion.Euler(frontRight) * Vector3.forward * 10.0f;
        Vector3 b_Right = Quaternion.Euler(backRight) * Vector3.forward * 10.0f;
        f_Right += transform.position;
        b_Right += transform.position;

        Vector3 f_Left = Quaternion.Euler(frontLeft) * Vector3.forward * 10.0f;
        Vector3 b_Left = Quaternion.Euler(backLeft) * Vector3.forward * 10.0f;
        f_Left += transform.position;
        b_Left += transform.position;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, f_Left);
        Gizmos.DrawLine(transform.position, b_Left);
        Gizmos.DrawLine(transform.position, f_Right);
        Gizmos.DrawLine(transform.position, b_Right);
        Gizmos.DrawWireSphere(transform.position + (Quaternion.Euler(transform.eulerAngles) * fire_position), 1.0f);


    }

    public override List<Vector3> GetTargetDirections()
    {
        List<Vector3> result = new List<Vector3>();

        Vector3 origin = transform.position;
        origin.y -= 10.0f;
        Vector3 originLeft =  origin - transform.right * 10.0f;
        Vector3 originRight = origin + transform.right * 10.0f;

        Vector3 endLeft =   originLeft + transform.forward * 1000.0f;
        Vector3 endRight =  originRight + transform.forward * 1000.0f;


        result.Add(originLeft);
        result.Add(originRight);
        result.Add(endLeft);
        result.Add(endRight);


        return result;
    }

    public override void Reload()
    {
        stateMachine.Transition((int)AimedTorpedoStateMachine.StateEnum.Ready);
    }

}
