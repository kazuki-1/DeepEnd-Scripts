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
        if (!CheckAmmo() || Pause.Get().IsPaused())
            return;
        Vector3 dist = transform.position - target.position;
        float time = dist.magnitude / (MainController.Get().maximumTorpedoSpeed * Time.deltaTime);
        Math.Round(time, 2);
        foreach (ArmamentList armamentList in armaments)
        {
            foreach (ArmamentBase armament in armamentList.list)
            {

                float distanceToTarget = Vector3.Distance(armament.transform.position, target.position);
                float timeToTarget = distanceToTarget / MainController.Get().maximumTorpedoSpeed;
                Vector3 target_pos = MainController.Get().SimulateVectorMovement(target.position, target.GetComponent<DeepEndPlayerController>().GetMovementVector()/*Vector3.zero*/, timeToTarget);





                if (armament.GetState() && armament.CheckDirectionToTarget(target_pos, 10.0f))
                    armament.Fire();
            }
        }
    }


    public override bool CheckAmmo()
    {
        return munitions.aimedTorpedo >= 0;
     
    }
}
