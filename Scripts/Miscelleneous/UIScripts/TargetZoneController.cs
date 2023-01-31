using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetZoneController : MonoBehaviour
{
    [HideInInspector]
    public ArmamentBase armament;

    [SerializeField]
    Quad quad;

    private void Start()
    {
        //lineObj.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void SetPosition(ArmamentBase armament)
    {
        List<Vector3> points = armament.GetTargetDirections();
        quad.SetVertices(points);
    }
    public void Enable()
    {
        quad.gameObject.SetActive(true);
        quad.Enable();
    }

    public void Disable()
    {
        quad.gameObject.SetActive(false);
        quad.Disable();
    }

}
