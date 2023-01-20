using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmamentBase : MonoBehaviour
{

    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*------------------------------------Variables-----------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    protected AK.Wwise.Event fireEvent;

    [HideInInspector]
    protected StateMachineBase stateMachine;

    [SerializeField]
    public Vector3 fire_position;       // Starting position of ordinance path

    [SerializeField]
    public int barrel_count = 3;        // How many ordinances are fired per trigger

    [HideInInspector]
    public float reload_time = 10;        // How long to reload armament

    [HideInInspector]
    public Vector3 fire_direction;      // Ordinance direction

    [SerializeField]
    public float angleLimit = 45.0f;    // 

    [SerializeField]
    public Vector2 UIPosition;

    


    protected Vector3 starting_position;
    protected Vector3 starting_direction;
    protected Vector3 target_direction;

    [HideInInspector]
    public bool isReloading = false;

    public bool outOfBounds { get; protected set; } = false;

    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*------------------------------------Functions-----------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/

    public virtual void Update()
    {
        stateMachine.Execute();
    }

    public virtual void Reload() { }

    public virtual void OnDrawGizmosSelected()
    {

        Vector3 leftEuler, rightEuler;
        leftEuler = transform.eulerAngles;
        leftEuler.y += angleLimit;

        rightEuler = transform.eulerAngles;
        rightEuler.y += -angleLimit;


        Vector3 left = Quaternion.Euler(leftEuler) * Vector3.forward * 10.0f;
        Vector3 right = Quaternion.Euler(rightEuler) * Vector3.forward * 10.0f;
        left += transform.position;
        right += transform.position;


        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, left);
        Gizmos.DrawLine(transform.position, right);
        Gizmos.DrawWireSphere(transform.position + (Quaternion.Euler(transform.eulerAngles) * fire_position), 1.0f);

    }

    public virtual void Fire() { }

    public Vector3 GetFirePosition()
    {
        return fire_position;    

    }

    public virtual Vector3 GetLocalFirePosition()
    { 
        return transform.localPosition + fire_position;
    }
    public Vector3 GetFireDirection()
    {

        // TODO : Get fire direction from mouse
        Vector3 dir = transform.forward;

        return dir;
        
    }

    public Vector3 GetUIPosition()
    {
        return new Vector3(UIPosition.x, UIPosition.y, 0.0f);
    }

    /// <summary>
    /// Rotates the armament to the point
    /// </summary>
    /// <param name="point"></param>
    public virtual void RotateToPoint(Vector3 point)
    {

        Vector3 p = new Vector3(point.x, 0.0f, point.z);                            // Target point
        Vector3 o = new Vector3(starting_direction.x, 0.0f, starting_direction.z);  // Origin

        float angle_diff = Vector3.Angle(p.normalized, o.normalized);
        if (Mathf.Abs(angle_diff) > angleLimit)
        {
            outOfBounds = true;
            return;
        }
        else
            outOfBounds = false;

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        float threshold = MainController.Get().armamentAngleDifferenceThreshold;
        if (Mathf.Abs(Vector3.Angle(transform.forward.normalized, forward)) > threshold)
            outOfBounds = true;


        Quaternion q = Quaternion.LookRotation(forward, transform.up);
        target_direction = point.normalized;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, 0.003f);


    }
    

    public StateMachineBase GetStateMachine()
    {
        return stateMachine;
    }

    /// <summary>
    ///  Returns true if armament is ready to be fired
    /// </summary>
    /// <returns></returns>
    public virtual bool GetState()
    {
        return !isReloading && !outOfBounds;
    }

    public virtual float GetRemainingReloadTime() { return 0.0f; }

    private void OnDrawGizmos()
    {
        GameObject obj = GameObject.Find("Canvas");
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(obj.transform.position + new Vector3(UIPosition.x, UIPosition.y, 0.0f), new Vector3(50.0f, 50.0f, 0.0f));
    }

    /// <summary>
    /// Used for drawing target UI widget
    /// </summary>
    /// <returns></returns>
    virtual public List<Vector3> GetTargetDirections() { return new List<Vector3>(); }

    /// <summary>
    /// Only use this for enemies. Targets the armament to the target parameter
    /// </summary>
    /// <param name="target"></param>
    public void Target(Transform target)
    {
        Vector3 dir = starting_direction;
        Vector3 target_dir = target.position - transform.position;

        // Checks if target is within angle limit
        if (Vector3.Angle(dir.normalized, target_dir.normalized) > angleLimit)
            return;


        transform.rotation = Quaternion.LookRotation(target_dir.normalized);
    }

    /// <summary>
    /// Only use this for enemies. Checks if the target is equal or close to the forward vector of this object
    /// </summary>
    /// <returns></returns>
    public bool CheckDirectionToTarget(Transform target, float angle = 10.0f)
    {
        return CheckDirectionToTarget(target.position, angle);
    }

    public bool CheckDirectionToTarget(Vector3 target, float angleThreshold = 10.0f)
    {
        Vector3 dir = target- transform.position;
        dir.Normalize();
        float angle = Vector3.Angle(transform.forward, dir);
        if (Vector3.Angle(transform.forward, dir) > angleThreshold)
            return false;
        return true;
    }

}
