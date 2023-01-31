using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeepEndEntityController : MonoBehaviour
{

    /*------------------------------------------------------------*/
    /*-----------------------------Variables----------------------*/
    /*------------------------------------------------------------*/


    [SerializeField]
    public StateMachineBase stateMachine;

    [SerializeField]
    public int healthPoints = 10;


    /*------------------------------------------------------------*/
    /*-----------------------------Functions----------------------*/
    /*------------------------------------------------------------*/



    public void Destroy()
    {
        GameObject.Destroy(gameObject);
        GetComponentInChildren<AudioInterface>().Pause();
    }


    public virtual void TakeDamage(int dmg)
    {
        healthPoints -= dmg;
    }

    /// <summary>
    /// Returns true if unit is still alive
    /// </summary>
    /// <returns></returns>
    public bool IsAlive()
    {
        return healthPoints >= 0;


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

    public StateMachineBase GetStateMachine()
    {
        return stateMachine;
    }
}
