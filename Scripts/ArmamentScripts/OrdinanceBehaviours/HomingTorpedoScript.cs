using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used on the homingTorpedoes themselves rather than the tubes they are launched from
/// </summary>
public class HomingTorpedoScript : MonoBehaviour
{

    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*---------------------------------Variables----------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/


    [HideInInspector]
    public Transform target;

    [HideInInspector]
    public float beginning_propulsion = 60.0f;      // Used when they are launched from tubes

    [HideInInspector]
    public Vector3 initial_direction;               

    private Vector3 movement;

    private bool hasHitWater = false;
    private bool isArmed = false;
    private float speed;
    private float despawnTimer;
    private float armingTime;

    private Timer timer;


    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*---------------------------------Functions----------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/



    // Start is called before the first frame update
    void Start()
    {

        MainController.ArmamentParameters parameter = MainController.Get().homingTorpedoParameters;
        timer = new Timer((int)parameter.despawnTime);

        initial_direction = target.position - transform.position;
        initial_direction.Normalize();
        transform.LookAt(transform.position + initial_direction * 10.0f);
         movement = initial_direction * beginning_propulsion * Time.deltaTime;
        speed = parameter.speed;

        armingTime = MainController.Get().torpedoArmingTime;
        


    }

    // Update is called once per frame
    void Update()
    {

        if (timer.OnPass(armingTime))
            isArmed = true;
        
        // Moment after launching from tubes
        if (!hasHitWater)
        {
            movement.x *= .9f;
            movement.z *= .9f;

            movement.y -= 1.0f * Time.deltaTime;
        }
        // Underwater
        else
        {
            Vector3 distance = target.position - transform.position;
            Vector3 direction = distance.normalized;

            if (speed < MainController.Get().maximumTorpedoSpeed)
            {
                speed += MainController.Get().homingTorpedoParameters.speed;
            }
            movement = transform.forward * speed * Time.deltaTime;
            movement.y = 0;
        }
        transform.position += movement;

        Vector3 dist = target.position - transform.position;
        dist.Normalize();

        if (Vector3.Dot(transform.forward, dist) > 0)
        {
            Quaternion lk = Quaternion.LookRotation(dist);
            transform.rotation = Quaternion.Slerp(transform.rotation, lk, 0.05f);
        }

        timer.Execute();
        if (timer.Done())
            GameObject.Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterScript>() != null)
            hasHitWater = true;

        if (!isArmed)
            return;

        if (other.gameObject.GetComponent<DeepEndEntityController>() != null)
        {
            other.gameObject.GetComponent<DeepEndEntityController>().TakeDamage(MainController.Get().homingTorpedoParameters.damage);
            Object.Destroy(gameObject);

        }

    }

}
