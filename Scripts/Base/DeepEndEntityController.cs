using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeepEndEntityController : MonoBehaviour
{
    [SerializeField]
    public StateMachineBase stateMachine;

    [SerializeField]
    public int healthPoints = 10;

    public void TakeDamage(int dmg)
    {
        healthPoints -= dmg;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    virtual public void Transition(int next_)
    {
        stateMachine.Transition(next_);
    }
}
