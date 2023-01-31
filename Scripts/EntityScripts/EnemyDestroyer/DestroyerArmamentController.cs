using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerArmamentController : ArmamentController
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


    public override void Start()
    {
        base.Start();

        // Sets the player as the target
        target = GameObject.FindWithTag("Player").transform;
    }
    public override void Update()
    {
        //SelectArmament();
        //FireArmaments();
        RotateArmament();

    }

    public override void FireArmaments()
    {
        if (Pause.Get().IsPaused() || !CheckAmmo())
            return;


        foreach (ArmamentList armamentList in armaments)
        {
            foreach(ArmamentBase armament in armamentList.list)
            {
                if (armament.GetState() && armament.CheckDirectionToTarget(target))
                    armament.Fire();

            }
        }
    }

    

    public override void RotateArmament()
    {

        Vector3 direction = transform.position - target.position;
        foreach(ArmamentList list in armaments)
        {
            foreach (ArmamentBase armament in list.list)
            {
                // Aims the armaments at the player constantly
                armament.Target(target);
            }
        }

    }

    public override bool CheckAmmo()
    {
        return munitions.cannons >= 0;
    }

    public override Vector3 GetDirection()
    {
        return transform.forward;
    }

}
