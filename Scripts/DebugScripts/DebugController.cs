using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        pos += transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        transform.position = pos;




    }
}
