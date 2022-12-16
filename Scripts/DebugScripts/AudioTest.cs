using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        transform.Translate(Vector3.right * Time.deltaTime * 100.0f);
        transform.LookAt(target.position);



    }
}
