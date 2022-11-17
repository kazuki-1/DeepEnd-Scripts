using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GameObject parent = null;
            switch (list.type)
            {
                case ArmamentController.Armaments.Cannon:
                    parent = cannonGaugeParent;
                    break;
                case ArmamentController.Armaments.AimedTorpedo:
                    parent = aimedTorpedoGaugeParent;
                    break;
                case ArmamentController.Armaments.HomingTorpedo:
                    parent = homingTorpedoGaugeParent;
                    break;
            }

            foreach (var item in list.list)
            {
                GameObject obj = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Gauge"), parent.transform);
                obj.GetComponent<GaugeScript>().armament = item;
                Vector2 pos = obj.GetComponent<RectTransform>().anchoredPosition;
                pos.x = item.UIPosition.x;
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
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
                aimedTorpedoGaugeParent.SetActive(false);
                homingTorpedoGaugeParent.SetActive(false);
                break;
            case ArmamentController.Armaments.AimedTorpedo:
                cannonGaugeParent.SetActive(false);
                aimedTorpedoGaugeParent.SetActive(true);
                homingTorpedoGaugeParent.SetActive(false);
                break;
            case ArmamentController.Armaments.HomingTorpedo:
                cannonGaugeParent.SetActive(false);
                aimedTorpedoGaugeParent.SetActive(false);
                homingTorpedoGaugeParent.SetActive(true);
                break;
        }


        foreach (var obj in GetComponentsInChildren<UIPanelInterface>())
        {
            if (playerArmamentController.GetCurrentArmament() == obj.GetArmamentType())
            {
                selection.transform.position = Vector3.Lerp(selection.transform.position, obj.transform.position, 0.03f);
            }
        }
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
