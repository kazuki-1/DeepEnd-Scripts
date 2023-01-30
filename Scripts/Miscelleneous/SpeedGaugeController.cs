using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeController : MonoBehaviour
{
    public GameObject slider;

    GameObject player;      // Use this to get acceleration stage

    int maxAccel;
    int curAccel;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        maxAccel = player.GetComponent<DeepEndPlayerController>().accelerationStageCount;
    }

    // Update is called once per frame
    void Update()
    {
        Slider c_Slider = slider.GetComponent<Slider>();
        curAccel = player.GetComponent<DeepEndPlayerController>().accel_state;
        float val = (float)curAccel / (float)maxAccel;
        c_Slider.value = Mathf.Lerp(c_Slider.value, val, Time.deltaTime * 5);
    }
}
