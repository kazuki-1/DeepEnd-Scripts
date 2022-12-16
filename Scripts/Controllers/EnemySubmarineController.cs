using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySubmarineController : DeepEndEnemyController
{
    public float attackRange = 100.0f;
    public float submergeTime = 60.0f;
    [HideInInspector]
    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        if (stateMachine == null)
            stateMachine = new SubmarineStateMachine();
        stateMachine.Initialize(gameObject);
        target = GameObject.FindWithTag("Player").transform;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stateMachine != null)
            stateMachine.Execute();

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
