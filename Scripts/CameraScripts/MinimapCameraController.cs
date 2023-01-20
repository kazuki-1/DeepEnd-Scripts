using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    // Start is called before the first frame update

    [HideInInspector]
    public Vector3 offset;

    // Used for deactivating the minimap objects
    List<GameObject> enemies = new List<GameObject> ();
    static public MinimapCameraController Get()
    {
        GameObject cam = GameObject.Find("MiniMapViewport");
        return GameObject.Find("MiniMapViewport").GetComponent<MinimapCameraController>();
    }

    public void Register(GameObject obj)
    {
        Light comp = obj.GetComponent<Light>();

        // Check children
        if(comp == null)
            comp = obj.GetComponentInChildren<Light>();

        // Check parent
        if(comp == null)
            comp = obj.GetComponentInParent<Light>();

        enemies.Add(comp.gameObject);
    }

    public void HideAll()
    {
        foreach (GameObject obj in enemies)
            obj.SetActive(false);
    }

    public void ShowAll()
    {
        foreach (GameObject obj in enemies)
            obj.SetActive(true);
    }

    void Start()
    {
        
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + target.position;
        //GetComponent<Camera>().cullin
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, target.position);

    }
}
