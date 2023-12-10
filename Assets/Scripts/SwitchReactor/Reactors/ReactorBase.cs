using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReactorBase : MonoBehaviour, IReactor
{
    public abstract void PerformReaction();
    public abstract void EndReaction();
}
