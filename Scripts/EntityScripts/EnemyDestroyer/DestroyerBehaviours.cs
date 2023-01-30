using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyerStates
{

    public abstract class DestroyerState_Base : StateBase
    {
        public DeepEndEnemyController controller;
        protected StateMachineBase GetStateMachine()
        {
            return controller.GetComponent<EnemyDestroyerController>().stateMachine;
        }
        protected void Transition(int nextState_)
        {
            GetStateMachine().Transition(nextState_);
        }
    
    }



    public class DestroyerState_Approach : DestroyerState_Base
    {
        private Transform target;
        private Vector3 direction;
        private Quaternion target_rotation;

        public override void Initialize(GameObject parent)
        {
        }

        public override void Execute(GameObject parent)
        {
            GameObject player = GameObject.Find("DeepEndPlayer");
            EnemyDestroyerController controller = parent.GetComponent<EnemyDestroyerController>();

            Transform transform = parent.transform;
            target = player.transform;
            controller.movementSpeed = Mathf.Lerp(controller.movementSpeed, controller.maximumSpeed, 0.05f);


            direction = target.position - transform.position;
            target_rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, 0.7f * Time.deltaTime);
            transform.position += parent.transform.forward * controller.movementSpeed * Time.deltaTime;



            float distance = Vector3.Magnitude(target.position - transform.position);
            if (distance  < controller.firingRange)
                Transition((int)DestroyerStateMachine.StateEnum.PursuitAndAttack);

            if (!controller.IsAlive())
                Transition((int)DestroyerStateMachine.StateEnum.Sink);



        }

        public override void End(GameObject parent)
        {
            target = null;
            direction = Vector3.zero;
            target_rotation = Quaternion.identity;
        }



    }

    public class DestroyerState_PursuitAndAttack : DestroyerState_Base
    {

        private Transform target;
        private Vector3 direction;
        private Quaternion target_rotation;


        public override void Initialize(GameObject parent)
        {

        }

        public override void Execute(GameObject parent)
        {

            GameObject player = GameObject.Find("DeepEndPlayer");

            Transform transform = parent.transform;
            target = player.transform;
            controller.movementSpeed = Mathf.Lerp(controller.movementSpeed, controller.maximumSpeed, 0.05f);


            direction = target.forward;
            target_rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, 0.7f * Time.deltaTime);
            transform.position += parent.transform.forward * controller.movementSpeed * Time.deltaTime;
            controller.GetComponent<ArmamentController>().FireArmaments();

            if (!controller.GetComponent<DestroyerArmamentController>().CheckAmmo())
                    Transition((int)DestroyerStateMachine.StateEnum.Retreat);

            if (!controller.IsAlive())
                Transition((int)DestroyerStateMachine.StateEnum.Sink);

        }

        public override void End(GameObject parent)
        {
            target = null;
            direction = Vector3.zero;
            target_rotation = Quaternion.identity;

        }



    }

    /// <summary>
    /// Unused
    /// </summary>
    public class DestroyerState_Ramming : DestroyerState_Base
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

    public class DestroyerState_Sink : DestroyerState_Base
    {

        public override void Initialize(GameObject parent)
        {
            //controller.Sink();
            // Placeholder
            MainController.Get().GetStats().LogSunk(EnemySpawner.EnemyType.Destroyer);
            controller.Destroy();
        }

        public override void Execute(GameObject parent)
        {




        }

        public override void End(GameObject parent)
        {

        }



    }

    public class DestroyerState_Retreat : DestroyerState_Base
    {
        Vector3 direction;
        public override void Initialize(GameObject parent)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            direction = parent.transform.position - player.transform.position;
            direction.Normalize();
        }

        public override void Execute(GameObject parent)
        {

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            parent.transform.position += direction * Time.deltaTime;

            if (!controller.IsAlive())
                Transition((int)DestroyerStateMachine.StateEnum.Sink);


            if ((parent.transform.position - player.transform.position).magnitude > 2500.0f)
                End(parent);



        }

        public override void End(GameObject parent)
        {
            controller.GetComponent<DeepEndEntityController>().Destroy();
        }

    }

    public class DestroyerState_Debug : DestroyerState_Base
    {
        List<Vector3> points = new List<Vector3>();
        float distance = 1000.0f;
        int vertexCount = 256;
        Transform target;
        int cur_index = 0;
        public override void Initialize(GameObject parent)
        {

            
            target = GameObject.FindWithTag("Player").transform;
            distance = (parent.transform.position - target.position).magnitude;
            Vector3 center = target.position;
            float anglePerCycle = (float)vertexCount / 360.0f;
            Vector3 closest_point = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 pos = parent.transform.position;
            
            for(int ind = 0; ind < vertexCount; ++ind)
            {

                Vector3 point;
                point.x = Mathf.Cos(ind * anglePerCycle) * distance + center.x;
                point.z = Mathf.Sin(ind * anglePerCycle) * distance + center.z;
                point.y = 0;
                points.Add(point);

                Vector3 dist = (pos - point);
                if (dist.magnitude < (closest_point - pos).magnitude)
                {
                    closest_point = point;
                    cur_index = ind;
                }
            }

        }

        public override void Execute(GameObject parent)
        {

            if (!controller.IsAlive())
                Transition((int)DestroyerStateMachine.StateEnum.Sink);

            //Transform transform = parent.transform;
            //Vector3 dest = points[cur_index];
            //transform.LookAt(points[cur_index]);
            //transform.position = transform.position + transform.forward * 0.7f;
            //if ((transform.position - dest).magnitude < 10.0f)
            //    ++cur_index;
            //if (cur_index > points.Count - 1)
            //    cur_index = 0;

        }

        public override void End(GameObject parent)
        {

        }

    }
}
