using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float radius = 1000;

    [Tooltip("How many vertices does the circle have")]
    public int vertexCount= 50;

    public List<GameObject> enemyPrefabs;


    List<Vector3> spawnPoints = new List<Vector3>();

    Timer timer = new Timer();


    // Start is called before the first frame update
    void Start()
    {
        Vector3 center = transform.position;
        float anglePerCycle = (float)vertexCount / 360.0f;
        Vector3 closest_point = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        for (int ind = 0; ind < vertexCount; ++ind)
        {

            Vector3 point;
            point.x = Mathf.Cos(ind * anglePerCycle) * radius + center.x;
            point.z = Mathf.Sin(ind * anglePerCycle) * radius + center.z;
            point.y = 0;
            spawnPoints.Add(point);

        }

    }

    // Update is called once per frame
    void Update()
    {
        timer.Execute();

        if (timer.OnEveryPass(10.0f))
            Spawn(0);



    }

    void Spawn(int itr)
    {
        GameObject obj = Instantiate<GameObject>(enemyPrefabs[itr], spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.green;
        Vector3 center = transform.position;
        float anglePerCycle = 360.0f / (float)vertexCount;
        Vector3 closest_point = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

        for (int ind = 0; ind < vertexCount; ++ind)
        {

            Vector3 point;
            point.x = Mathf.Cos(ind * anglePerCycle) * radius + center.x;
            point.z = Mathf.Sin(ind * anglePerCycle) * radius + center.z;
            point.y = 0;
            Gizmos.DrawWireSphere(point, 30.0f);
        }

    }
}
