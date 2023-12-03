using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public abstract class SwitchBase : MonoBehaviour
{
    [SerializeField]
    List<ReactorBase> reactors = new();

    readonly protected UnityEvent switchActivated = new();
    readonly protected UnityEvent switchDeactivated = new();

    private void Awake()
    {
        foreach (IReactor reactor in reactors)
        {
            switchActivated.AddListener(reactor.PerformReaction);
            switchDeactivated.AddListener(reactor.EndReaction);
        }
    }

    protected virtual bool CollisionValid(Collision2D collision)
    {
        return collision.gameObject.layer != LayerMask.NameToLayer("default");
    }
}
