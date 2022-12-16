using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubmarineStates
{

    public abstract class SubmarineState_Base : StateBase
    {
        protected EnemySubmarineController controller;
        protected StateMachineBase GetStateMachine()
        {
            return controller.GetComponent<EnemyDestroyerController>().stateMachine;
        }
        public override void Initialize(GameObject parent)
        {
            if (controller == null)
                controller = parent.GetComponent<EnemySubmarineController>();

        }
        protected void Transition(int nextState_)
        {
            GetStateMachine().Transition(nextState_);
        }


    }


    public class SubmarineState_Approach : SubmarineState_Base
    {
        float cur_speed = 0.0f;
        Timer timer;
        Transform target;

        public override void Initialize(GameObject parent)
        {
            base.Initialize(parent);

            timer = new Timer((int)controller.submergeTime);
            cur_speed = 0.0f;


        }

        public override void Execute(GameObject parent)
        {


            cur_speed += (controller.maximumSpeed / 10.0f) * Time.deltaTime;        // takes 10 seconds to reach maximum speed;
            cur_speed = Mathf.Clamp(cur_speed, 0.0f, controller.maximumSpeed);

            Vector3 distance = controller.target.transform.position - controller.transform.position;
            Vector3 direction = distance.normalized;

            Quaternion rot = Quaternion.FromToRotation(controller.transform.forward, direction);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rot, 0.7f * Time.deltaTime);
            Vector3 pos = controller.transform.position;
            pos += controller.transform.forward * cur_speed;
            controller.transform.position = pos;

            timer.Execute();

            if (distance.magnitude < controller.attackRange)
                Transition((int)SubmarineStateMachine.StateEnum.PursuitAndAttack);


        }

        public override void End(GameObject parent)
        {
            timer.Reset();
        }



    }

    public class SubmarineState_Surface : SubmarineState_Base
    {

        public override void Initialize(GameObject parent)
        {

        }

        public override void Execute(GameObject parent)
        {

        }

        public override void End(GameObject parent)
        {

        }



    }

    public class SubmarineState_Submerge : SubmarineState_Base
    {

        public override void Initialize(GameObject parent)
        {

        }

        public override void Execute(GameObject parent)
        {

        }

        public override void End(GameObject parent)
        {

        }



    }

    public class SubmarineState_PursuitAndAttack : SubmarineState_Base
    {

        public override void Initialize(GameObject parent)
        {

        }

        public override void Execute(GameObject parent)
        {

        }

        public override void End(GameObject parent)
        {

        }



    }

    public class SubmarineState_Sunk : SubmarineState_Base
    {

        public override void Initialize(GameObject parent)
        {

        }

        public override void Execute(GameObject parent)
        {

        }

        public override void End(GameObject parent)
        {

        }



    }

    public class SubmarineState_Retreat : SubmarineState_Base
    {

        public override void Initialize(GameObject parent)
        {

        }

        public override void Execute(GameObject parent)
        {

        }

        public override void End(GameObject parent)
        {

        }



    }

}
