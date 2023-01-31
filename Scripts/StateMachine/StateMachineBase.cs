using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBase
{

    public GameObject parent;
    protected Dictionary<int, StateBase> states = new Dictionary<int, StateBase>();

    public StateBase curState_ { get; private set; }



    // Update is called once per frame
    public virtual void Execute()
    {
        
        if (curState_ != null)
            curState_.Execute(parent);
    }

    public virtual void Initialize() { }

    public virtual void Initialize(GameObject obj)
    {
        states.Clear();
        this.parent = obj;
        Initialize();
    }

    public virtual void Transition(int next_)
    {
        if (curState_ != null)
            curState_.End(parent);

        curState_ = states[next_];
        curState_.Initialize(parent);


    }

    public void Destroy()
    {
        states.Clear();
        Object.Destroy(parent.GetComponent<DeepEndEntityController>().gameObject);
    }

    ~StateMachineBase()
    {
        foreach (var state in states)   
            state.Value.End(parent);
    }

    // Returns the StateEnum in int form, return -1 if invalid
    public int GetStateEnum()
    {
        foreach(var state in states)
        {
            if (curState_ == state.Value)
                return state.Key;
        }
        return -1;
    }

}
