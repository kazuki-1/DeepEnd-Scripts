using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingTorpedoTubeController : ArmamentBase
{

    public Vector2 deadZoneAngle;

    private Transform target_transform;

    public Transform target { get; private set; }

    public float aimAngle = 30.0f;

    [HideInInspector]
    public bool isTargeting = false;

    [HideInInspector]
    public bool justFired = false;
    // Start is called before the first frame update
    void Awake()
    {
        angleLimit = 180.0f;
        //aimAngle = 180.0f;
        // Prep stateMachine
        stateMachine = new HomingTorpedoStateMachine();
        (stateMachine as HomingTorpedoStateMachine).controller = this;
        stateMachine.Initialize(gameObject);

        // Check for armamentController;
        ArmamentController armamentController = gameObject.GetComponentInParent<ArmamentController>();
        if (armamentController != null)
            reload_time = armamentController.reloadTimes.homingTorpedo;
        else reload_time = 45;
        (stateMachine as HomingTorpedoStateMachine).reload_time = reload_time;

        starting_direction = transform.forward;
        starting_position = transform.position;



    }

    // Update is called once per frame
    //void Update()
    //{
    //    stateMachine.Execute();
    //}

    public override void Fire()
    {
        if (outOfBounds)
            return;
        if (!isReloading)
            stateMachine.Transition((int)HomingTorpedoStateMachine.StateEnum.Target);

    }

    public void PostSFXEvent()
    {
        fireEvent.Post(gameObject);
    }

    public override void OnDrawGizmosSelected()
    {
        // Angle Limit
        // Prepping the rotation eulers
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

        // Transforms it to the gameObj pos
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


        // Aiming angle 
        Gizmos.color = Color.green;
        Vector3 left, right;
        left = right = transform.eulerAngles;
        left.y -= aimAngle;
        right.y += aimAngle;

        left = Quaternion.Euler(left) * Vector3.forward * 10.0f;
        right = Quaternion.Euler(right) * Vector3.forward * 10.0f;
        left += transform.position;
        right += transform.position;

        Gizmos.DrawLine(transform.position, left);
        Gizmos.DrawLine(transform.position, right);



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

    public override float GetRemainingReloadTime()
    {
        HomingTorpedoStateMachine.StateEnum state = (HomingTorpedoStateMachine.StateEnum)stateMachine.GetStateEnum();
        if (state == HomingTorpedoStateMachine.StateEnum.Reload)
            return (stateMachine.curState_ as HomingTorpedoStates.HomingTorpedoState_Reload).timer.GetRemainingTime();
        return 0.0f;

    }

    // This is overrided because homingTorpedoes have an additional targeting state
    public override bool GetState()
    {
        return base.GetState() && !isTargeting /*&& !justFired*/;
    }
    public void SetTarget(Transform obj)
    {
        target = obj;
    }

    public void ClearTarget()
    {
        target = null;
    }

    // Used when cancelling targeting phase
    public void ClearState()
    {
        isTargeting = isReloading = false;
    }

    public override List<Vector3> GetTargetDirections()
    {
        List<Vector3> result = new List<Vector3>();

        Vector3 left, right;
        left = right = transform.eulerAngles;
        left.y -= aimAngle;
        right.y += aimAngle;

        Vector3 origin = transform.position;
        origin.y -= 10.0f;
        left = Quaternion.Euler(left) * Vector3.forward * 1000.0f;
        right = Quaternion.Euler(right) * Vector3.forward * 1000.0f;
        left +=     origin;
        right +=    origin;



        result.Add(origin);
        result.Add(left);

        result.Add(right);
        result.Add(origin);

        return result;
    }

    public override void Reload()
    {
        stateMachine.Transition((int)HomingTorpedoStateMachine.StateEnum.Ready);
    }


}
