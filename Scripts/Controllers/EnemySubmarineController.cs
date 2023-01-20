using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubmarineController : DeepEndEnemyController
{
    public float attackRange = 500.0f;                  
    public float minimumRange = 100.0f;                 // Stops if within minimum range
    public float maximumSubmergeTime = 60.0f;           // Time the sub can remain submerged
    public float surfaceTime = 10.0f;                   // Time the sub takes to surface
    public float submergeHeight = 40.0f;                // How deep should the sub submerge to               
    [HideInInspector]
    public Transform target;

    Timer submergeTimer;
    bool isSubmerged = false;
    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        if (stateMachine == null)
            stateMachine = new SubmarineStateMachine();
        stateMachine.Initialize(gameObject);
        target = GameObject.FindWithTag("Player").transform;
        stateMachine.Transition((int)SubmarineStateMachine.StateEnum.Submerge);
        submergeTimer = new Timer(maximumSubmergeTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine != null)
            stateMachine.Execute();

        if (isSubmerged)
            submergeTimer.Execute();

    }

    /// <summary>
    /// Begin submergeTimer
    /// </summary>
    public void BeginTimer()
    {
        isSubmerged = true;
    }

    /// <summary>
    /// Stops submergeTimer
    /// </summary>
    public void StopTimer()
    {
        isSubmerged = false;
    }

    /// <summary>
    /// Resets the submerge timer
    /// </summary>
    public void ResetTimer()
    {
        submergeTimer.Reset();
    }
    /// <summary>
    /// Returns true if timer is done
    /// </summary>
    /// <returns></returns>
    public bool SubmergeTimerDone()
    {
        return submergeTimer.Done();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minimumRange);
    }

}
