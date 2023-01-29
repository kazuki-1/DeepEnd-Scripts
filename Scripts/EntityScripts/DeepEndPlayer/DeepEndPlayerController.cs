using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DeepEndPlayerController : DeepEndEntityController
{

    


    [SerializeField]
    private float static_speed = 50.0f;

    [SerializeField]
    private float incremental_speed;

    [SerializeField]
    private Vector2 speedLimit = new Vector2(-5.0f, 15.0f);

    

    

    [SerializeField]
    private float smoothness = 0.05f;

    [SerializeField]
    private int accelerationStageCount = 5;

    [HideInInspector]
    static public Vector3 movement = new Vector3();      // Player velocity
    static private int accel_state;     // Stage of acceleration
    static private float acceleration_flatRate;    // Acceleration per frame
    private Vector3 default_speed;
    Timer radarTimer;

    // Start is called before the first frame update
    void Start()
    {
        movement.z = static_speed;
        default_speed = movement;

        // How much speed is changed everytime you speed up or down
        acceleration_flatRate = incremental_speed / (float)accelerationStageCount;

        if (stateMachine == null)
            stateMachine = new DeepEndPlayerStateMachine();
        stateMachine.Initialize(gameObject);

        radarTimer = new Timer(MainController.Get().radarCooldownTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Movement
        {
            float speed = Input.GetAxis("Vertical");

            Controls();


            // Speed control
            movement = transform.forward * (static_speed + (acceleration_flatRate * accel_state)) ;
            if(accel_state == 0)
                movement = Vector3.Lerp(movement, default_speed, smoothness);

            //Mathf.Clamp(movement, speedLimit.x, speedLimit.y);


            movement *= Time.deltaTime;

            //Vector3 position = transform.position;
            //position += movement;
            //transform.position = position;



        }

        radarTimer.Execute();
        DebugControls();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

    }

    void DebugControls()
    {
        // Scene Reloading
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Scene cur_Scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(cur_Scene.name);
            }
        }

        // Reload armaments
        else if (Input.GetKeyDown(KeyCode.R))
            GetComponent<ArmamentController>().ReloadAll();

    }

    void Controls()
    {
        // Acceleration controls
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

        // Radar Controls
        {
            if (Input.GetKeyDown(KeyCode.F) && radarTimer.Done())
            {
                radarTimer.Reset();
                Sonar.Get().Activate();
            }
        }
    }

    public Vector3 GetMovementVector()
    {
        return movement;
    }

}
