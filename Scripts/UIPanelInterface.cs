using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelInterface : MonoBehaviour
{

    [SerializeField]
    private ArmamentController.Armaments type;

    public ArmamentController.Armaments GetArmamentType()
    {
        return type;
    }
}
