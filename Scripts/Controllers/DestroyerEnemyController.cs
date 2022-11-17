using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerEnemyController : DeepEndEnemyController
{
    // Start is called before the first frame update
    void Start()
    {
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
}
