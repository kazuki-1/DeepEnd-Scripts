using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [HideInInspector]
    Vector3 start_position;

    [HideInInspector]
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        start_position = transform.position;
        offset = start_position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(target.position);


    }
}
