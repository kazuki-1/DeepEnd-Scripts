using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRayCaster : MonoBehaviour
{

    public Transform target;
    public AkAudioListener listener;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Vector3 dist = target.position - transform.position;

        Physics.Raycast(transform.position, dist.normalized, out hit);

        if (hit.collider == null || hit.collider.tag != "Wall")
            AkSoundEngine.SetObjectObstructionAndOcclusion(gameObject, listener.gameObject, 0.0f, 0.0f);
        else 
            AkSoundEngine.SetObjectObstructionAndOcclusion(gameObject, listener.gameObject, 100.0f, 100.0f);






    }
}
