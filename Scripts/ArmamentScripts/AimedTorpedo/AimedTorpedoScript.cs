using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedTorpedoScript : MonoBehaviour
{

    /*---------------------------------------------------------------------------------*/
    /*--------------------------------------Variables----------------------------------*/
    /*---------------------------------------------------------------------------------*/


    public GameObject explosionParticlePrefab;

    public AK.Wwise.Event explosionSoundEvent;

    public GameObject waterTrailEffect;


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
    Timer timer;

    /*----------------------------------------------------------------------------------*/
    /*---------------------------------------Functions----------------------------------*/
    /*----------------------------------------------------------------------------------*/


    // Start is called before the first frame update
    void Start()
    {
        MainController.ArmamentParameters parameter = MainController.Get().aimedTorpedoParameters;
        movement = direction * beginning_propulsion * Time.deltaTime;
        speed = parameter.speed;
        timer = new Timer(parameter.despawnTime);
        MainController.Get().GetStats().LogFired(ArmamentController.Armaments.AimedTorpedo);

        waterTrailEffect.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

        // Movement
        {
            // Moment after launching from tubes
            if (!hasHitWater)
            {
                movement.x *= .9f;
                movement.z *= .9f;

                movement.y -= 1.0f;
            }
            // Underwater
            else
            {
                Vector3 pos = waterTrailEffect.GetComponent<Transform>().position;
                pos.y = 0.0f;
                waterTrailEffect.GetComponent<Transform>().position = pos;

                if (movement.magnitude < MainController.Get().maximumTorpedoSpeed)
                {
                    movement += direction * speed ;
                    movement.y = 0;
                }
            }
        }

        if (!hasCollided)
            transform.position += movement * Time.deltaTime;


        // Destroy conditions
        {
            timer.Execute();
            // Despawns if on stage too long without hitting anything
            if (timer.Done())
                Object.Destroy(gameObject);

            // Despawns after explosion is finished
            if (explosionParticle && (!explosionParticle.GetComponent<ParticleSystem>().IsAlive() && hasCollided))
                Object.Destroy(gameObject);

        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaterScript>() != null)
        {
            hasHitWater = true;
            movement.y = 0;
            waterTrailEffect.SetActive(true);
        }
        if (other.gameObject == source)
            return;




        Transform transform = GetComponent<Transform>();
        if (other.gameObject.GetComponent<DeepEndEntityController>() != null)
        {

            if (other.gameObject.name == "DeepEndPlayer")
                MainController.Get().GetStats().LogTorpedoHit();


            other.gameObject.GetComponent<DeepEndEntityController>().TakeDamage(MainController.Get().aimedTorpedoParameters.damage);

            // Instantiates the particle system
            explosionParticle = Instantiate<GameObject>(explosionParticlePrefab, transform);
            explosionParticle.GetComponent<ParticleSystem>().Play();
            explosionParticle.transform.localScale = new Vector3(30, 30, 30);
            explosionParticle.transform.localPosition = Vector3.zero;
            transform.position = other.transform.position;
            hasCollided = true;
            waterTrailEffect.SetActive(false);

            // Posts the collision sfx
            explosionSoundEvent.Post(gameObject);


            GetComponentInChildren<MeshRenderer>().enabled = false;
            MainController.Get().GetStats().LogHit(ArmamentController.Armaments.AimedTorpedo);


            //Object.Destroy(gameObject);
        }

    }

}
