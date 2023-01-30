using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TorpedoIndicator : MonoBehaviour
{
    [SerializeField]
    Texture2D torpedoTexture;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject torpedoUI;
    

    //public int vertices = 50;
    public float radius = 200.0f;

    //List<Vector3> points = new List<Vector3>();
    //Vector2 closestPoint;
    // Start is called before the first frame update
    void Start()
    {
        torpedoUI.GetComponent<RawImage>().texture = torpedoTexture;
        torpedoUI.SetActive(false);
        //float anglePerCycle = 360 / vertices;
        //Vector2 center = Vector2.zero;

        //for (int ind = 0; ind < vertices; ind++)
        //{ 
        //    Vector2 point = Vector2.zero;
        //    point.x = Mathf.Cos(ind * anglePerCycle) * radius + center.x;
        //    point.y = Mathf.Sin(ind * anglePerCycle) * radius + center.y;
        //    points.Add(point);

        //}


    }

    // Update is called once per frame
    void Update()
    {
        bool detected = false;

        GameObject[] torpedoes = GameObject.FindGameObjectsWithTag("Torpedo");
        if (torpedoes.Length < 0)
            return;
        RectTransform transform = torpedoUI.GetComponent<RectTransform>();

        Vector3 playerPos = player.transform.position;
        Vector2 v2_PlayerPos;
        v2_PlayerPos.x = playerPos.x;
        v2_PlayerPos.y = playerPos.z;

        
        foreach(GameObject torpedo in torpedoes)
        {
            Vector3 torpPos = torpedo.transform.position;

            if ((playerPos - torpPos).magnitude > 300.0f)
                continue;

            torpPos.y = 0;
            Vector2 v2_torpPos;
            v2_torpPos.x = torpPos.x;
            v2_torpPos.y = torpPos.z;

            Vector2 dist = v2_torpPos - v2_PlayerPos;
            transform.anchoredPosition = dist.normalized * radius;
            float angle = Vector3.Angle(player.transform.forward, torpPos.normalized);
            transform.eulerAngles = new Vector3(0, 0, -angle);
            detected = true;
            break;


        }
        if (detected)
            torpedoUI.SetActive(true);
        else
            torpedoUI.SetActive(false);
    }
}
