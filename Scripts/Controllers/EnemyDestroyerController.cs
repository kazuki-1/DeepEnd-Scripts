using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyerController : DeepEndEnemyController
{

    [Tooltip("Range for switching to attack state")]
    public float firingRange = 100.0f;

    [Tooltip("Range for switching to ramming state")]
    public float rammingRange = 20.0f;


    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        if (stateMachine == null)
            stateMachine = new DestroyerStateMachine();
        stateMachine.Initialize(gameObject);
        stateMachine.Transition((int)DestroyerStateMachine.StateEnum.Debug);
    }

    // Update is called once per frame
    void Update()
    {
        if(stateMachine != null)
            stateMachine.Execute();
        //transform.position = transform.position + transform.forward * 0.5f;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, firingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rammingRange);
    }

}
