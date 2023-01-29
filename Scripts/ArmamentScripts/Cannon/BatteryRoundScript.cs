using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryRoundScript : MonoBehaviour
{
    public GameObject explosionParticlePrefab;

    public AK.Wwise.Event explosionSoundEvent;

    [HideInInspector]
    public GameObject source;       // Where the round is fired from
    Vector3 start_position;

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

    private bool hasCollided = false;

    private GameObject explosionParticle;

    void Start()
    {
        MainController.ArmamentParameters parameter = GameObject.Find("MainController").GetComponent<MainController>().batteryParameters;
        despawnTimer = parameter.despawnTime;
        velocity = direction.normalized * parameter.speed * Time.deltaTime;
        transform.LookAt(transform.position + direction * 10.0f);

        MainController.Get().GetStats().LogFired(ArmamentController.Armaments.Cannon);

    }

    // Update is called once per frame
    void Update()
    {

        // Movement 
        {
            if (!hasCollided)
            {
                // Velocity decays
                velocity.y -= 0.1f * Time.deltaTime;
                transform.position += velocity;
            }
            else
                velocity = Vector3.zero;

        }

        // Destroy conditions
        {
            // Deletes the object if it has existed for too long
            despawnTimer -= Time.deltaTime;
            if (despawnTimer < 0)
                Object.Destroy(gameObject);


            // Also deletes the object if the explosion has finished rendering
            if (explosionParticle && (!explosionParticle.GetComponent<ParticleSystem>().IsAlive() && hasCollided))
                Object.Destroy(gameObject);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Prevents the round from colliding with the source
        if (other == null || other.gameObject == source)
            return;


        if(other.gameObject.GetComponent<DeepEndEntityController>())
        {

            // Instantiates the particle system
            other.gameObject.GetComponent<DeepEndEntityController>().TakeDamage(MainController.Get().batteryParameters.damage);
            explosionParticle = Instantiate<GameObject>(explosionParticlePrefab, transform);
            explosionParticle.GetComponent<ParticleSystem>().Play();
            explosionParticle.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
            explosionParticle.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            hasCollided = true;

            GetComponentInChildren<MeshRenderer>().enabled = false;

            // Posts the collision SFX
            explosionSoundEvent.Post(gameObject);

            MainController.Get().GetStats().LogHit(ArmamentController.Armaments.Cannon);
        }


    }



    private void OnDrawGizmos()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
