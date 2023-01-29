using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stats))]
[Tooltip("Contains global constants" +
    "Also contains miscellenous functions")]
public class MainController : MonoBehaviour
{
    
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*------------------------------------Variables-----------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/

    [System.Serializable]
    public class ArmamentParameters
    {
        [Tooltip("Ordinance speed")]
        public float speed;
        public float despawnTime;
        public int damage;
        public AK.Wwise.Event fireSFXEvent;

    }

    public class Colours
    {
        static public Color32 blank = new Color32(0, 0, 0, 0);
        static public Color32 black = new Color32   (0, 0, 0, 255);
        static public Color32 grey = new Color32    (50, 50, 50, 255);
        static public Color32 red = new Color32     (255, 0, 0, 255);
        static public Color32 green = new Color32   (0, 255, 0, 255);
        static public Color32 blue = new Color32    (0, 0, 255, 255);
        static public Color32 yellow = new Color32  (255, 255, 0, 255);
        static public Color32 white = new Color32   (255, 255, 255, 255);


    }


    private List<GameObject> audioSources = new List<GameObject> ();

    public ArmamentParameters batteryParameters;

    public ArmamentParameters aimedTorpedoParameters;

    public ArmamentParameters homingTorpedoParameters;

    [Tooltip("Maximum range acquisition for homing torpedoes")]
    public float maximumTorpedoRange;       
    [Tooltip("Torpedoes will accelerate towards this")]
    public float maximumTorpedoSpeed;
    [Tooltip("Time needed for torpedoes to arm from launch. In Seconds")]
    public float torpedoArmingTime;

    public float armamentAngleDifferenceThreshold = 10.0f;

    public Vector2 waveformDimensions;

    public int radarCooldownTime;

    public float radarEffectTime;

    private Timer timer;    // Timer for vector simulation

    public AK.Wwise.Event pauseAllEvents;
    public AK.Wwise.Event resumeAllEvents;


    bool isPaused = false;


    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*------------------------------------Functions-----------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/

    private void Start()
    {
        Cursor.visible = false;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AudioSource"))
            audioSources.Add(obj);
    }
    static public MainController Get()
    {
        return GameObject.Find("MainController").GetComponent<MainController>();
    }

    public Stats GetStats()
    {
        return GetComponent<Stats>();
    }

    /// <summary>
    /// Generates a waveform texture based on the audio clip
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public Texture2D GenerateTextureFromAudio(AudioClip audio, int width, int height)
    {
        Texture2D output = new Texture2D(width, height, TextureFormat.RGBA32, false);
        AudioClip clip = audio;

        float[] data = new float[clip.samples * clip.channels];
        clip.GetData(data, 0);
        

        double dataPerPixel = 2.0f / height;          // The waveform is limited to -1.0f ~ 1.0f. This will dictate the height of the samples
        int samplePerPixel = (int)(data.Length / width);
        int center_index = (int)(height / 2.0f);      // Since the data goes from -1.0f ~ 1.0f, 0.0f will be the center

        // Texture params
        int size = width * height;
        Color32[] pixels = new Color32[(width * height)];



        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                pixels[x + y * width] = Colours.grey;


        for (int x = 0; x < width; ++x)
        {

            if (samplePerPixel * x > data.Length - 1)
                break;
            float sample = data[x * (int)samplePerPixel];

            int sample_height = (int)(Mathf.Abs(sample / (float)dataPerPixel));
            int cur_ind = center_index * width + x;




            for (int y = 0; y < sample_height; ++y)
            {
                int below, above;
                below = cur_ind - y * width; 
                above = cur_ind + y * width;

                

                if (below >= 0 && below < size)
                    pixels[below] = Colours.yellow;

                if (above >= 0 && above < size)
                    pixels[above] = Colours.yellow;
                 
            }


        }


        output.SetPixels32(pixels);
        output.Apply();




        return output;


    }

    //public void MuteAllExcept(GameObject target)
    //{
    //    foreach (GameObject obj in audioSources)
    //    {
    //        obj.GetComponent<AudioSource>().mute = false;
    //        if (obj == target)
    //            continue;
    //        obj.GetComponent<AudioSource>().mute = true;
    //    }
    //}

    //public void UnmuteAll()
    //{
    //    foreach (GameObject obj in audioSources)
    //        obj.GetComponent<AudioSource>().mute = false;
    //}

    /// <summary>
    /// Perform calculation to give a point based on a simulation
    /// of a rough estimate of where the point will be 
    /// after a specified time
    /// </summary>
    /// <param name="start"></param>
    /// <param name="velocity"></param>
    /// <param name="time"> Simulation time</param>
    /// <returns></returns>
    public Vector3 SimulateVectorMovement(Vector3 start, Vector3 velocity, float time)
    {
        float deltaTime = Time.deltaTime;
        return start + velocity * time;

    }

    public void PauseAll()
    {
        // if (isPaused)
        //     return;
        //pauseAllEvents.Post(gameObject);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AudioSource"))
            obj.GetComponent<AudioInterface>().Pause();
        isPaused = true;

    }

    public void ResumeAll()
    {
        if (!isPaused)
            return;
        // resumeAllEvents.Post(gameObject);

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AudioSource"))
            obj.GetComponent<AudioInterface>().Post();
        isPaused = false;

    }



}
