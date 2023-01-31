using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*------------------------------------------------------------*/
    /*-----------------------------Variables----------------------*/
    /*------------------------------------------------------------*/

    [System.Serializable]
    public class Entity
    {
        public EnemyType type;
        public GameObject entityPrefab;
        public int spawnRatio = 5;
    }

    public enum EnemyType
    {
        Destroyer, 
        Submarine
    };

    [SerializeField]
    bool enableSpawning = true;

    [SerializeField]
    float radius = 1000;

    [SerializeField]
    float spawnInterval = 10.0f;


    [Tooltip("How many vertices does the circle have")]
    public int vertexCount= 50;

    public List<Entity> entityLibrary = new List<Entity>();
    Dictionary<EnemyType, Entity> entities = new Dictionary<EnemyType, Entity>();
    public int enemyCount = 30;
    public int maxEnemyOnField = 4;


    int constValue = 0;
    List<Vector3> spawnPoints = new List<Vector3>();

    Timer timer = new Timer();

    /*------------------------------------------------------------*/
    /*-----------------------------Functions----------------------*/
    /*------------------------------------------------------------*/

    static public EnemySpawner Get()
    {
        return GameObject.Find("SpawningSystem").GetComponent<EnemySpawner>();
    }


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
        foreach (var entity in entityLibrary)
        {
            entities.Add(entity.type, entity);
            constValue += entity.spawnRatio;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For debug use
        if (!enableSpawning)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");



        timer.Execute();

        // if there are too many enemies on field
        if (enemies.Length < maxEnemyOnField)
        {

            if (timer.OnEveryPass(spawnInterval))
            {
                int randNum = Random.Range(0, constValue - 1);      // We use this to determine what will be spawned
                int index = 0;
                int min = -1;
                int max = 0;
                foreach (var entity in entities)
                {
                    max += entity.Value.spawnRatio;
                    if (randNum > min && randNum < max)
                        Spawn(entity.Key);
                    min += entity.Value.spawnRatio;
                    ++index;
                }
            }
        }
    }

    void Spawn(EnemyType itr)
    {
        GameObject obj = Instantiate<GameObject>(entities[itr].entityPrefab, spawnPoints[Random.Range(0, spawnPoints.Count)], Quaternion.identity);
        MainController.Get().GetStats().LogSpawned(itr);
    }

    public GameObject Spawn(EnemyType itr, Vector3 pos)
    {
        GameObject obj = Instantiate<GameObject>(entities[itr].entityPrefab, pos, Quaternion.identity);
        MainController.Get().GetStats().LogSpawned(itr);
        return obj;
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
