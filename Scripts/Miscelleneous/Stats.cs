using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    /*------------------------------------------------------------*/
    /*-----------------------------Variables----------------------*/
    /*------------------------------------------------------------*/


    public class ArmamentStats
    {
        public int fired = 0;
        public int hit = 0;
        public float hitPercentage = 0.0f;

    }

    public class EnemyStats
    {
        public int count = 0;
        public int sunkCount = 0;

    }



    public int cannonHitsTaken = 0;
    public int torpedoHitsTaken = 0;
    public int damageTaken = 0;

    public Dictionary<ArmamentController.Armaments, ArmamentStats> stats = new Dictionary<ArmamentController.Armaments, ArmamentStats>();
    public Dictionary<EnemySpawner.EnemyType, EnemyStats> enemyStats = new Dictionary<EnemySpawner.EnemyType, EnemyStats>();

    /*------------------------------------------------------------*/
    /*-----------------------------Functions----------------------*/
    /*------------------------------------------------------------*/



    // Start is called before the first frame update
    void Start()
    {
        stats.Add(ArmamentController.Armaments.Cannon,          new ArmamentStats());
        stats.Add(ArmamentController.Armaments.AimedTorpedo,    new ArmamentStats());
        stats.Add(ArmamentController.Armaments.HomingTorpedo,   new ArmamentStats());

        enemyStats.Add(EnemySpawner.EnemyType.Destroyer, new EnemyStats());
        enemyStats.Add(EnemySpawner.EnemyType.Submarine, new EnemyStats());
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Logs the ordinance to calculate the hit ratio
    /// </summary>
    /// <param name="type"></param>
    public void LogFired(ArmamentController.Armaments type)
    {
        stats[type].fired++;
    }

    /// <summary>
    /// Logs the ordinance to calculate the hit ratio
    /// </summary>
    /// <param name="type"></param>
    public void LogFired(ArmamentController.Armaments type, int count)
    {
        stats[type].fired += count;
    }

    /// <summary>
    /// Logs the ordinance to calculate the hit ratio
    /// </summary>
    /// <param name="type"></param>
    public void LogHit(ArmamentController.Armaments type)
    {
        stats[(type)].hit++;
    }

    /// <summary>
    /// Logs the ordinance to calculate the hit ratio
    /// </summary>
    /// <param name="type"></param>
    public void LogHit(ArmamentController.Armaments type, int count)
    {
        stats[(type)].hit += count;
    }

    public void LogSpawned(EnemySpawner.EnemyType type)
    {
        enemyStats[type].count++;
    }

    public void LogSunk(EnemySpawner.EnemyType type)
    {
        enemyStats[type].sunkCount++;
    }

    public void LogDamageTaken(int dmg)
    {
        damageTaken += dmg;
    }

    public void LogCannonHit()
    {
        cannonHitsTaken++;
    }
    public void LogTorpedoHit()
    {
        torpedoHitsTaken++;
    }

    public void FinalizeStats()
    {

        foreach (var entity in stats.Values)
            entity.hitPercentage = (float)entity.hit / (float)entity.fired;



    }
}
