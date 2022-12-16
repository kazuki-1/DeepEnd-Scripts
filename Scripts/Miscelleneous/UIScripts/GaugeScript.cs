using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GaugeScript : MonoBehaviour
{

    [SerializeField]
    private Color reloadingColour;

    [SerializeField]
    private Color readyColour;

    [SerializeField]
    private TMPro.TextMeshProUGUI text;


    [HideInInspector]
    public ArmamentBase armament;
    bool prev_state = false;        // Use this to check if the armament was in a different state in the previous frame
    bool transition = false;

    // Start is called before the first frame update
    void Start()
    {
        prev_state = armament.GetState();
    }

    // Update is called once per frame
    void Update()
    {
        bool state = armament.GetState();
        if (prev_state != state)
            transition = true;

        if(transition == true)
        {
            Image img = GetComponent<Image>();
            switch (state)
            {
                case true:
                    img.color = Color.Lerp(img.color, readyColour, 0.05f);
                    if (img.color == readyColour)
                        transition = false;
                    break;
                case false:
                    img.color = Color.Lerp(img.color, reloadingColour, 0.05f);
                    if(img.color == reloadingColour)
                        transition= false;
                    break;
            }


            
        }

        float reloading_time = armament.GetRemainingReloadTime();
        double rounded_time = Math.Round(reloading_time, 1);
        text.text = rounded_time.ToString();




        prev_state = state;
    }
}
