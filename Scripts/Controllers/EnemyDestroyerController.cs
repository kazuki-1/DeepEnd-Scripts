using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyerController : DeepEndEnemyController
{

    [Tooltip("Range for switching to attack state")]
    public float firingRange = 100.0f;

    [Tooltip("Range for switching to ramming state")]
    public float rammingRange = 20.0f;

    [Tooltip("Does not enable stateMachine to execute")]
    public bool debugMode = false;

    [Tooltip("Damage applied to player when ramming")]
    public int rammingDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        if (stateMachine == null)
            stateMachine = new DestroyerStateMachine();
        stateMachine.Initialize(gameObject);
        if (debugMode)
            stateMachine.Transition((int)DestroyerStateMachine.StateEnum.Debug);
        else
            stateMachine.Transition((int)DestroyerStateMachine.StateEnum.Approach);
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

    private void OnTriggerEnter(Collider other)
    {
        DeepEndEntityController player = other.gameObject.GetComponent<DeepEndPlayerController>();
        if (player)
        {
            player.TakeDamage(rammingDamage);
            stateMachine.Transition((int)DestroyerStateMachine.StateEnum.Sink);
        }
    }

}
