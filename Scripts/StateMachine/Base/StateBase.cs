using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{

    // Called when changing to this state
    public abstract void Initialize(GameObject parent);
    // Called every frame 
    public abstract void Execute(GameObject parent);

    // Called before changing states
    public abstract void End(GameObject parent);

}
