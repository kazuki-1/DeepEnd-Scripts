using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CannonStates
{

    abstract class CannonState_Base : StateBase
    {
        public CannonController controller;
        protected StateMachineBase GetStateMachine()
        {
            return controller.GetStateMachine();
        }
        protected void Transition(int next_)
        {
            GetStateMachine().Transition(next_);
        }
        
    }
    class CannonState_Ready : CannonState_Base
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

    class CannonState_Fire : CannonState_Base
    {
        public override void Initialize(GameObject parent)
        {
            Transform transform = parent.transform;

            for(int i = 0; i < controller.barrel_count; ++i)
            {
                GameObject ord = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/BatteryRound"));
                Vector3 fire_pos = controller.GetFirePosition();
                fire_pos.x += (float)(-1 * 2.0f + i);
                fire_pos = Quaternion.Euler(transform.eulerAngles) * fire_pos;
                //fire_pos = transform.position + fire_pos;
                ord.transform.position = transform.position + fire_pos;
                //ord.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                ord.GetComponent<BatteryRoundScript>().direction = controller.GetFireDirection();
                //ord.GetComponent<BatteryRoundScript>().Initialize() ;

            }



        }

        public override void Execute(GameObject parent)
        {

            controller.GetStateMachine().Transition((int)CannonStateMachine.StateEnum.Reload);


        }

        public override void End(GameObject parent)
        {
        }


    }

    class CannonState_Reload : CannonState_Base
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
                Transition((int)CannonStateMachine.StateEnum.Ready);
        }

        public override void End(GameObject parent)
        {
            controller.isReloading = false;
            
        }


    }


}
