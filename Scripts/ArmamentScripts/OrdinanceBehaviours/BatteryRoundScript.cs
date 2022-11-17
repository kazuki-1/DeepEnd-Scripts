using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryRoundScript : MonoBehaviour
{

    public float Decay(float f, float decayRate)
    {
        return f * decayRate;
    }

    public Vector3 Decay(Vector3 v, float decayRate)
    {
        return v * ( 1.0f - decayRate * Time.deltaTime); 
    }


    // Start is called before the first frame update
    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public float maximum_distance = 1000.0f;

    private float despawnTimer;

    private Vector3 velocity;

    void Start()
    {
        MainController.ArmamentParameters parameter = GameObject.Find("MainController").GetComponent<MainController>().batteryParameters;
        despawnTimer = parameter.despawnTime;
        velocity = direction.normalized * parameter.speed * Time.deltaTime;
        transform.LookAt(transform.position + direction * 10.0f);
    }

    // Update is called once per frame
    void Update()
    {

        // Velocity decays
        //velocity = Decay(velocity, .9999f);
        velocity.y -= 0.1f * Time.deltaTime;

        transform.position += velocity;

        // Deletes the object if it has existed for too long
        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0)
            Object.Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;


        if(other.gameObject.GetComponent<DeepEndEntityController>())
        {
            // TODO  Damage entity
            other.gameObject.GetComponent<DeepEndEntityController>().TakeDamage(MainController.Get().batteryParameters.damage);
            Object.Destroy(gameObject);

        }


    }



    private void OnDrawGizmos()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
