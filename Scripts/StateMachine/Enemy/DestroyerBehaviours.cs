using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyerStates
{
    public class DestroyerState_Approach : StateBase
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

            direction =  target.position - transform.position;


            target_rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);




            transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, 0.7f * Time.deltaTime);
            

            parent.transform.position += parent.transform.forward * 0.5f;


            float distance = Vector3.Magnitude(target.position - transform.position);
            if (distance < 10.0f)
                parent.GetComponent<DeepEndEntityController>().Transition((int)DestroyerStateMachine.StateEnum.PursuitAndAttack);

        }

        public override void End(GameObject parent)
        {

        }



    }

    
    public class DestroyerState_PursuitAndAttack : StateBase
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

    public class DestroyerState_Ramming : StateBase
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

    public class DestroyerState_Sunk : StateBase
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

    public class DestroyerState_Debug : StateBase
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
            Transform transform = parent.transform;
            Vector3 dest = points[cur_index];
            transform.LookAt(points[cur_index]);
            transform.position = transform.position + transform.forward * 0.7f;
            if ((transform.position - dest).magnitude < 10.0f)
                ++cur_index;
            if (cur_index > points.Count - 1)
                cur_index = 0;
            
        }

        public override void End(GameObject parent)
        {

        }

    }
}
