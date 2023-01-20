using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimedTorpedoStates
{

    abstract class AimedTorpedState_Base : StateBase
    {
        public AimedTorpedoTubeController controller;
        protected StateMachineBase GetStateMachine()
        {
            return controller.GetStateMachine();
        }
        protected void Transition(int next_)
        {
            GetStateMachine().Transition(next_);
        }
    }

    class AimedTorpedoState_Ready : AimedTorpedState_Base
    {
        public override void Initialize(GameObject parent)
        {
        }

        public override void Execute(GameObject parent)
        {

            // Transition is done from controller

        }

        public override void End(GameObject parent)
        {
        }


    }

    class AimedTorpedoState_Fire : AimedTorpedState_Base
    {
        public override void Initialize(GameObject parent)
        {
            Transform transform = parent.transform;
            for (int i = 0; i < controller.barrel_count; ++i)
            {

                // Creates the ordinance and initializes its parameters
                GameObject ord = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AimedTorpedo"));
                Vector3 fire_pos = controller.GetFirePosition();
                fire_pos.x += (float)(-1 * 2.0f + i);
                fire_pos = Quaternion.Euler(transform.eulerAngles) * fire_pos;
                ord.transform.position = transform.position + fire_pos;
                ord.GetComponent<AimedTorpedoScript>().direction = controller.GetFireDirection();
                ord.transform.rotation = Quaternion.LookRotation(controller.GetFireDirection());

                // Sets the source as the parent gameObject so as to not let it collide with itself
                ord.GetComponent<AimedTorpedoScript>().source = controller.GetComponentInParent<DeepEndEntityController>().gameObject;

            }
            Transition((int)AimedTorpedoStateMachine.StateEnum.Reload);

        }

        public override void Execute(GameObject parent)
        {
            // DONE : Fix states not transitioning after firing

        }

        public override void End(GameObject parent)
        {
        }


    }

    class AimedTorpedoState_Reload : AimedTorpedState_Base
    {
        public Timer timer { get; private set; }
        public override void Initialize(GameObject parent)
        {
            controller.isReloading = true;
            if (timer == null)
                timer = new Timer(controller.reload_time);
            else
                timer.Reset();
        }

        public override void Execute(GameObject parent)
        {
            timer.Execute();
            if (timer.Done())
                Transition((int)AimedTorpedoStateMachine.StateEnum.Ready);
        }

        public override void End(GameObject parent)
        {
            
            controller.isReloading = false;
        }


    }



}
