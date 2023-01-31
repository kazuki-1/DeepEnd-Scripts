using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*---------------------------------Variables----------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/




    [SerializeField]
    public ArmamentController playerArmamentController;

    [HideInInspector]
    private ArmamentController.Armaments selected_armament;

    [SerializeField]
    private GameObject selection;

    [SerializeField]
    private GameObject cannonGaugeParent;

    [SerializeField]
    private GameObject aimedTorpedoGaugeParent;

    [SerializeField]
    private GameObject homingTorpedoGaugeParent;

    [SerializeField]
    private GameObject targetModule;

    [SerializeField]
    private GameObject targetPanel;

    [SerializeField]
    private GameObject cannonCrosshairs;

    [SerializeField]
    private GameObject aimedTorpedoCrosshairs;

    [SerializeField]
    private GameObject homingTorpedoCrosshairs;

    [SerializeField]
    GameObject cannonHitIndicator;

    [SerializeField]
    GameObject torpedoHitIndicator;

    [SerializeField]
    GameObject sonarCooldownTime;


    bool initialized = false;


    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/
    /*---------------------------------Functions----------------------------*/
    /*----------------------------------------------------------------------*/
    /*----------------------------------------------------------------------*/

    // Start is called before the first frame update
    void Start()
    {

    }

    void Initialize()
    {
        selection.SetActive(true);
        GenerateGauges();
        initialized = true;
        targetModule.SetActive(false);
        targetPanel.SetActive(false);
    }

    void GenerateGauges()
    {

        foreach (var list in playerArmamentController.armaments)
        {
            GameObject gaugeParent = null;
            GameObject crosshairParent = null;
            switch (list.type)
            {
                case ArmamentController.Armaments.Cannon:
                    gaugeParent = cannonGaugeParent;
                    crosshairParent = cannonCrosshairs;
                    break;
                case ArmamentController.Armaments.AimedTorpedo:
                    gaugeParent = aimedTorpedoGaugeParent;
                    crosshairParent = aimedTorpedoCrosshairs;
                    break;
                case ArmamentController.Armaments.HomingTorpedo:
                    gaugeParent = homingTorpedoGaugeParent;
                    crosshairParent = homingTorpedoCrosshairs;
                    break;
            }

            foreach (var item in list.list)
            {
                GameObject gauge = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Gauge"), gaugeParent.transform);
                gauge.GetComponent<GaugeScript>().armament = item;
                Vector2 pos = gauge.GetComponent<RectTransform>().anchoredPosition;
                pos.x = item.UIPosition.x;
                gauge.GetComponent<RectTransform>().anchoredPosition = pos;

                GameObject crosshair = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Crosshair"), crosshairParent.transform);
                crosshair.GetComponent<Crosshair>().armament = item;
                crosshair.GetComponent<Crosshair>().idlePosition = item.UIPosition;

            }




        }



    }



    // Update is called once per frame
    void Update()
    {
        // Perform initialization at the first frame
        if (!initialized)
            Initialize();


        selected_armament = playerArmamentController.GetCurrentArmament();


        
        // Reload gauges
        switch (selected_armament)
        {
            case ArmamentController.Armaments.Cannon:
                cannonGaugeParent.SetActive(true);
                cannonCrosshairs.SetActive(true);

                aimedTorpedoGaugeParent.SetActive(false);
                aimedTorpedoCrosshairs.SetActive(false);

                homingTorpedoGaugeParent.SetActive(false);
                homingTorpedoCrosshairs.SetActive(false);

                break;
            case ArmamentController.Armaments.AimedTorpedo:
                cannonGaugeParent.SetActive(false);
                cannonCrosshairs.SetActive(false);

                aimedTorpedoGaugeParent.SetActive(true);
                aimedTorpedoCrosshairs.SetActive(true);

                homingTorpedoGaugeParent.SetActive(false);
                homingTorpedoCrosshairs.SetActive(false);
                break;
            case ArmamentController.Armaments.HomingTorpedo:
                cannonGaugeParent.SetActive(false);
                cannonCrosshairs.SetActive(false);

                aimedTorpedoGaugeParent.SetActive(false);
                aimedTorpedoCrosshairs.SetActive(false);

                homingTorpedoGaugeParent.SetActive(true);
                homingTorpedoCrosshairs.SetActive(true);
                break;
        }


        foreach (var obj in GetComponentsInChildren<UIPanelInterface>())
        {
            if (playerArmamentController.GetCurrentArmament() == obj.GetArmamentType())
            {
                selection.transform.position = Vector3.Lerp(selection.transform.position, obj.transform.position, 0.03f);
            }
        }


        // Updates the number of bullets that hit
        cannonHitIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = MainController.Get().GetStats().stats[ArmamentController.Armaments.Cannon].hit.ToString();
        
        int torpCount = MainController.Get().GetStats().stats[ArmamentController.Armaments.HomingTorpedo].hit +
            MainController.Get().GetStats().stats[ArmamentController.Armaments.AimedTorpedo].hit;

        torpedoHitIndicator.GetComponent<TMPro.TextMeshProUGUI>().text = torpCount.ToString();

        // Updates the cooldown time for the sonar
        sonarCooldownTime.GetComponent<TMPro.TextMeshProUGUI>().text = Math.Round(Sonar.Get().GetCooldownTime(), 1).ToString();
        
    }

    public void ActivateTargetingModule()
    {
        targetModule.SetActive(true);
    }

    public void DeactivateTargetingModule()
    {
        targetModule.SetActive(false);
    }

    static public UIController Get()
    {
        return GameObject.Find("Canvas").GetComponent<UIController>();
    }

    public GameObject GetTargetPanel()
    {
        if (targetPanel != null)
            return targetPanel;
        return null;
    }

}
