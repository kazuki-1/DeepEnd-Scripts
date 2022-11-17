using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeepEndPlayerController : DeepEndEntityController
{

    
    float Lerp(float f1, float f2, float factor)
    {
        return f1 + (f2 - f1) * factor;
    }


    [SerializeField]
    private float static_speed = 5.0f;

    [SerializeField]
    private float incremental_speed;

    [SerializeField]
    private Vector2 speedLimit = new Vector2(-5.0f, 15.0f);


    [SerializeField]
    private float smoothness = 0.05f;

    [SerializeField]
    private int accelerationStageCount = 5;


    static private float movement;      // Player velocity
    static private int accel_state;     // Stage of acceleration
    static private float acceleration_flatRate;    // Acceleration per frame


    // Start is called before the first frame update
    void Start()
    {
        movement = static_speed;
        acceleration_flatRate = incremental_speed / (float)accelerationStageCount;

        if (stateMachine == null)
            stateMachine = new DeepEndPlayerStateMachine();
        stateMachine.Initialize(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
        // Movement
        {
            float speed = Input.GetAxis("Vertical");

            AccelControl();



            movement = static_speed + (acceleration_flatRate * accel_state);
            if(accel_state == 0)
                movement = Lerp(movement, static_speed, smoothness);

            Mathf.Clamp(movement, speedLimit.x, speedLimit.y);

            //Vector3 position = transform.position;
            //position.z += movement * Time.deltaTime;
            //transform.position = position;

            


        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Scene cur_Scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(cur_Scene.name);
            }
        }


        else if (Input.GetKeyDown(KeyCode.R))
            GetComponent<ArmamentController>().ReloadAll();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    void AccelControl()
    {
        string debug_accelstate = "Current Accel State = " + accel_state;
        string debug_speed = "Current accelerate = " + movement;

        if (Input.GetKeyDown(KeyCode.W))
        {
            accel_state++;
            Debug.Log(debug_accelstate);
            Debug.Log(debug_speed);

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            accel_state--;
            Debug.Log(debug_accelstate);
            Debug.Log(debug_speed);
        }



        accel_state = Mathf.Clamp(accel_state, -1, accelerationStageCount);
    }



}
