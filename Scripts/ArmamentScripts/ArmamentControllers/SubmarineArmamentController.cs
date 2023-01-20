using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SubmarineArmamentController : ArmamentController
{

    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*------------------------------Variable-------------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/


    Transform target;           // Player

    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*------------------------------Functions-------------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/




    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        target = GameObject.FindWithTag("Player").transform;

    }

    public override void Update()
    {

        //FireArmaments();
    }


    public override void FireArmaments()
    {
        if (!CheckAmmo())
            return;
        Vector3 dist = transform.position - target.position;
        float time = dist.magnitude / (MainController.Get().maximumTorpedoSpeed * Time.deltaTime);
        Math.Round(time, 2);
        Vector3 target_pos = MainController.Get().SimulateVectorMovement(target.position, target.GetComponent<DeepEndPlayerController>().GetMovementVector(), time);
        foreach (ArmamentList armamentList in armaments)
        {
            foreach (ArmamentBase armament in armamentList.list)
            {
                if (armament.GetState() && armament.CheckDirectionToTarget(target_pos, 30.0f))
                    armament.Fire();
            }
        }
    }

    /// <summary>
    /// Returns true if there are still munitions
    /// </summary>
    /// <returns></returns>
    //private void OnDrawGizmosSelected()
    //{
    //    Vector3 frontRight;
    //    frontRight = transform.eulerAngles;
    //    frontRight.y += fireAngle;

    //    Vector3 frontLeft;
    //    frontLeft = transform.eulerAngles;
    //    frontLeft.y -= fireAngle;

    //    // Transforms it to the gameObj pos
    //    Vector3 right = Quaternion.Euler(frontRight) * Vector3.forward * 100.0f;
    //    Vector3 left = Quaternion.Euler(frontLeft) * Vector3.forward * 100.0f;
    //    right += transform.position;
    //    left += transform.position;

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(transform.position, right);
    //    Gizmos.DrawLine(transform.position, left);


    //}

    public override bool CheckAmmo()
    {
        return munitions.aimedTorpedo >= 0;
     
    }
}
