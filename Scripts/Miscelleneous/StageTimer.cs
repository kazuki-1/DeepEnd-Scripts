using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StageTimer : MonoBehaviour
{

    /*------------------------------------------------------------*/
    /*-----------------------------Variables----------------------*/
    /*------------------------------------------------------------*/


    Timer timer;

    TMPro.TextMeshProUGUI text;

    string minutes = "";
    string seconds = "";

    /*------------------------------------------------------------*/
    /*-----------------------------Functions----------------------*/
    /*------------------------------------------------------------*/


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>(); 
        timer = new Timer(MainController.Get().stageTime);
    }

    // Update is called once per frame
    void Update()
    {

        int mins = (int)(timer.GetRemainingTime() / 60.0f);
        int secs = (int)(timer.GetRemainingTime() % 60.0f);

        minutes = mins.ToString();
        seconds = secs.ToString();

        text.text = minutes + " : " + seconds;


        timer.Execute();
    }

    public bool IsDone()
    {
        return timer.Done();
    }
}
