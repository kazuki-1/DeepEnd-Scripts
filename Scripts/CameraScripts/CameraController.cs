using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [HideInInspector]
    private Vector3 camera_offset;

    [HideInInspector]
    private Vector3 previous_position;

    [HideInInspector]
    private Vector3 mouse_pos = Vector3.zero;

    [HideInInspector]
    private float movement;

    // Start is called before the first frame update
    void Start()
    {
        camera_offset = target.position - transform.position;

    }


    // Update is called once per frame
    void Update()
    {
        // Lerps the camera position to somewhere behind and above the target

        //t += Time.deltaTime;


        transform.position = target.position - camera_offset;
        transform.LookAt(target);
        transform.RotateAround(target.position, target.transform.up, Input.GetAxis("MouseAxisX") * Time.deltaTime);
        camera_offset = target.position - transform.position;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(target.position, transform.position);
    }

    public Vector3 GetLookDirection()
    {
        return transform.forward;
    }
}
