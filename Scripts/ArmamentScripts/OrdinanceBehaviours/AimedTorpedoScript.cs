using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedTorpedoScript : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]
    public Vector3 direction = new Vector3(0, 0, 1);

    [HideInInspector]
    public float beginning_propulsion = 60.0f;

    private Vector3 movement;

    private bool hasHitWater = false;
    private float speed;
    float despawnTimer;
    void Start()
    {
        MainController.ArmamentParameters parameter = MainController.Get().aimedTorpedoParameters;
        despawnTimer = parameter.despawnTime;
        movement = direction * beginning_propulsion * Time.deltaTime;
        speed = parameter.speed;
    }


    // Update is called once per frame
    void Update()
    {


        // Moment after launching from tubes
        if (!hasHitWater)
        {
            movement = movement * 0.9f;
            movement.y -= 1.0f * Time.deltaTime;
        } 
        // Underwater
        else
        {
            if (movement.magnitude < MainController.Get().maximumTorpedoSpeed * Time.deltaTime)
            {
                movement += direction * speed  * Time.deltaTime;
                movement.y = 0;
            }
         }
        transform.position += movement;


        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0)
            Object.Destroy(gameObject);






    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterScript>() != null)
            hasHitWater = true;

        if(other.gameObject.GetComponent<DeepEndEntityController>() != null)
        {
            // TODO : Damage functions here
            other.gameObject.GetComponent<DeepEndEntityController>().TakeDamage(MainController.Get().aimedTorpedoParameters.damage);
            Object.Destroy(gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
    }
}
