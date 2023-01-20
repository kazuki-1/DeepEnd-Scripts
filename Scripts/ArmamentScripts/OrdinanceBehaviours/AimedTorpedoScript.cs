using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedTorpedoScript : MonoBehaviour
{

    /*---------------------------------------------------------------------------------*/
    /*--------------------------------------Variables----------------------------------*/
    /*---------------------------------------------------------------------------------*/


    public GameObject explosionParticlePrefab;

    [HideInInspector]
    public GameObject source;       // Where the round is fired from

    [HideInInspector]
    public Vector3 direction = new Vector3(0, 0, 1);

    [HideInInspector]
    public float beginning_propulsion = 60.0f;


    GameObject explosionParticle;
    Vector3 movement;
    bool hasHitWater = false;
    bool hasCollided = false;
    float speed;
    float despawnTimer;

    /*----------------------------------------------------------------------------------*/
    /*---------------------------------------Functions----------------------------------*/
    /*----------------------------------------------------------------------------------*/


    // Start is called before the first frame update
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
            movement.x *= .9f;
            movement.z *= .9f;

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

        // Despawns if on stage too long without hitting anything
        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0)
            Object.Destroy(gameObject);

        // Despawns after explosion is finished
        //if (explosionParticle && (!explosionParticle.GetComponent<ParticleSystem>().IsAlive() && hasCollided))
        //    Object.Destroy(gameObject);




    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterScript>() != null)
            hasHitWater = true;

        if (other.gameObject == source)
            return;





        if (other.gameObject.GetComponent<DeepEndEntityController>() != null)
        {
            // TODO : Damage functions here
            other.gameObject.GetComponent<DeepEndEntityController>().TakeDamage(MainController.Get().aimedTorpedoParameters.damage);


            explosionParticle = Instantiate<GameObject>(explosionParticlePrefab);
            explosionParticle.GetComponent<ParticleSystem>().Play();
            explosionParticle.transform.localScale = new Vector3(30.0f, 30.0f, 30.0f);
            //explosionParticle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 spawnPos = explosionParticle.transform.position;
            spawnPos.y = 0.0f;      // Causes the particle to spawn on the water plane
            hasCollided = true;

            GetComponentInChildren<MeshRenderer>().enabled = false;



            //Object.Destroy(gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
    }

}
