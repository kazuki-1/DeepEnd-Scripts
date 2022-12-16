using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ONLY USED FOR DEBUGGING
/// </summary>
[RequireComponent(typeof(AkSpatialAudioListener))]
public class AudioListener : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    SphereCollider audioCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position;
        //transform.rotation = GetComponentInParent<Transform>().parent.transform.rotation;
        if (audioCollider != null)
            audioCollider.center = target.position - GetComponentInParent<Transform>().position;
    }
}
