using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepEndEnemyController : DeepEndEntityController
{
    public float maximumSpeed = 30.0f;

    [HideInInspector]
    public float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    protected void Initialize()
    {
        // Registers the obj to the minimap controller so you can deactivate the minimap obj
        MinimapCameraController.Get().Register(gameObject);

    }
}
