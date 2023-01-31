using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubmarineStates
{

    public abstract class SubmarineState_Base : StateBase
    {
        public EnemySubmarineController controller;
        protected StateMachineBase GetStateMachine()
        {
            return controller.GetComponent<EnemySubmarineController>().stateMachine;
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

            Debug.Log("SubmarineState_Approach");
            

            base.Initialize(parent);

            cur_speed = 0.0f;
            controller.BeginTimer();



        }

        public override void Execute(GameObject parent)
        {


            cur_speed += (controller.maximumSpeed / 10.0f) * Time.deltaTime;        // takes 10 seconds to reach maximum speed;
            cur_speed = Mathf.Clamp(cur_speed, 0.0f, controller.maximumSpeed);

            Vector3 distance = controller.target.transform.position - controller.transform.position;
            Vector3 direction = distance.normalized;

            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, direction);
            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rot, 0.2f * Time.deltaTime);
            Vector3 pos = controller.transform.position;
            pos += (controller.transform.forward * cur_speed);
            controller.transform.position = pos;


            // Transitional conditions
            if (controller.SubmergeTimerDone())
                Transition((int)SubmarineStateMachine.StateEnum.Surface);

            if (distance.magnitude < controller.attackRange)
                Transition((int)SubmarineStateMachine.StateEnum.PursuitAndAttack);

            if (!controller.IsAlive())
                Transition((int)SubmarineStateMachine.StateEnum.Sink);


        }

        public override void End(GameObject parent)
        {
  
        }



    }

    public class SubmarineState_Surface : SubmarineState_Base
    {
        Vector3 submergedPosition;
        Vector3 surfacePosition;
        Vector3 velocity = Vector3.zero;
        Timer timer ;
        public override void Initialize(GameObject parent)
        {

            Debug.Log("SubmarineState_Surface");


            submergedPosition = parent.transform.position;
            surfacePosition = submergedPosition;
            surfacePosition.y = 0.5f;
            //timer = new Timer(controller.surfaceTime);
            controller.StopTimer();
            controller.ResetTimer();
            
            
        }

        public override void Execute(GameObject parent)
        {

            Vector3 diff = surfacePosition - submergedPosition;
            // Slowly surface the sub
            if (parent.transform.position.y > surfacePosition.y)
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * 2);
                if (timer == null)
                    timer = new Timer(controller.surfaceTime);
            }
            else if (velocity.magnitude < 5.0f)
            {
                   velocity += diff * (Time.deltaTime / controller.surfaceTime);
            }
            // 

            parent.transform.position += velocity * Time.deltaTime;


            if (timer != null)
                timer.Execute();

            if (timer != null && timer.Done())
                Transition((int)SubmarineStateMachine.StateEnum.Submerge);

            if (!controller.IsAlive())
                Transition((int)SubmarineStateMachine.StateEnum.Sink);
        }

        public override void End(GameObject parent)
        {
            timer = null;
            submergedPosition = surfacePosition = velocity = Vector3.zero;
        }



    }

    public class SubmarineState_Submerge : SubmarineState_Base
    {
        Vector3 submergedPosition;
        Vector3 surfacePosition;
        Vector3 velocity;
        Timer timer;

        public override void Initialize(GameObject parent)
        {

            Debug.Log("SubmarineState_Submerge");


            submergedPosition = surfacePosition = parent.transform.position;
            submergedPosition.y = -controller.submergeHeight;

        }

        public override void Execute(GameObject parent)
        {

            // Slowly submerge the sub
            Vector3 diff = submergedPosition - surfacePosition;

            // Slows the velocity when lowered to needed depth
            if (parent.transform.position.y < -controller.submergeHeight)
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime);
                if (velocity.magnitude < 0.1f)
                    Transition((int)SubmarineStateMachine.StateEnum.Approach);


            }
            else
                velocity += diff * (Time.deltaTime / controller.surfaceTime);

            parent.transform.position += velocity * Time.deltaTime;

            // Transitions when velocity stops
            if (velocity.magnitude < 0.01f)
                Transition((int)SubmarineStateMachine.StateEnum.Approach);

            if (!controller.IsAlive())
                Transition((int)SubmarineStateMachine.StateEnum.Sink);
        }

        public override void End(GameObject parent)
        {
            controller.BeginTimer();
            submergedPosition = surfacePosition = velocity = Vector3.zero;
        }



    }

    public class SubmarineState_PursuitAndAttack : SubmarineState_Base
    {
        float cur_speed = 0.0f;
        public override void Initialize(GameObject parent)
        {
            Debug.Log("SubmarineState_PursuitAndAttack");

            controller.BeginTimer();


        }

        public override void Execute(GameObject parent)
        {


            float f_distance = Vector3.Distance(parent.transform.position, controller.target.position);
            float timeToPosition = f_distance / MainController.Get().maximumTorpedoSpeed;
            Vector3 predictedPosition = controller.target.position + 
                                        controller.target.GetComponent<DeepEndPlayerController>().GetMovementVector() * timeToPosition;



            cur_speed += (controller.maximumSpeed / 10.0f) * Time.deltaTime;        // takes 10 seconds to reach maximum speed;
            cur_speed = Mathf.Clamp(cur_speed, 0.0f, controller.maximumSpeed);

            Vector3 distance = predictedPosition - controller.transform.position;
            Vector3 direction = distance.normalized;




            if (distance.magnitude > controller.minimumRange)
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, direction);
                controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rot, 0.2f * Time.deltaTime);
                controller.transform.position += controller.transform.forward * cur_speed * .25f;
            }
            controller.GetComponent<ArmamentController>().FireArmaments();

            // Transitional conditions
            if (!controller.IsAlive())
                Transition((int)SubmarineStateMachine.StateEnum.Sink);

            if (controller.SubmergeTimerDone())
                Transition((int)SubmarineStateMachine.StateEnum.Surface);

            if (!controller.GetComponent<SubmarineArmamentController>().CheckAmmo())
                Transition((int)SubmarineStateMachine.StateEnum.Retreat);

        }

        public override void End(GameObject parent)
        {

        }



    }

    public class SubmarineState_Sunk : SubmarineState_Base
    {
        GameObject fx;
        public override void Initialize(GameObject parent)
        {
            MainController.Get().GetStats().LogSunk(EnemySpawner.EnemyType.Destroyer);
            fx = GameObject.Instantiate(controller.defeatEffectPrefab, parent.transform);
            fx.transform.localScale = new Vector3(20, 20, 20);
            fx.transform.localPosition = Vector3.zero;
            controller.defeatSFXEvent.Post(parent);
        }

        public override void Execute(GameObject parent)
        {
            if (!fx.GetComponent<ParticleSystem>().IsAlive())
                End(parent);

        }

        public override void End(GameObject parent)
        {
            GetStateMachine().Destroy();
        }



    }

    public class SubmarineState_Retreat : SubmarineState_Base
    {
        Vector3 direction;
        public override void Initialize(GameObject parent)
        {
            Debug.Log("SubmarineState_Retreat");
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            direction = parent.transform.position - player.transform.position;
            direction.Normalize();
        }

        public override void Execute(GameObject parent)
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, direction);


            controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, rot, 0.1f * Time.deltaTime);
            parent.transform.position += parent.transform.forward * 10.0f * Time.deltaTime;

            if (!controller.IsAlive())
                Transition((int)SubmarineStateMachine.StateEnum.Sink);

            if ((parent.transform.position - player.transform.position).magnitude > 2500.0f)
                End(parent);

        }

        public override void End(GameObject parent)
        {
            controller.GetComponent<DeepEndEntityController>().Destroy();
        }

    }

}
