using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class OrientedAudioListener : MonoBehaviour
{

    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*------------------------------------Variables-----------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/


    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform cameraTransform;

    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*------------------------------------Functions-----------------------------------*/
    /*--------------------------------------------------------------------------------*/
    /*--------------------------------------------------------------------------------*/


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        Vector3 euler = cameraTransform.eulerAngles;
        euler.x = euler.z = 0;
        transform.localEulerAngles = euler;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.right* 100.0f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.up * 100.0f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.forward * 100.0f);


    }
}
