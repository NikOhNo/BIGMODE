using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverReactor : ReactorBase
{
    [SerializeField]
    Transform objectsToMove;

    [SerializeField]
    float moveTime = 1.5f;

    [SerializeField]
    Transform start;

    [SerializeField]
    Transform end;

    private void Awake()
    {
        objectsToMove.position = start.position;
    }

    public override void PerformReaction()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObjects(end));
    }

    public override void EndReaction()
    {
        StopAllCoroutines();
        StartCoroutine(MoveObjects(start));
    }

    IEnumerator MoveObjects(Transform location)
    {
        float elapsedTime = 0;

        Vector3 originalPosition = objectsToMove.position;
        while (elapsedTime < moveTime)
        {
            Vector3 newPos = Vector3.Lerp(originalPosition, location.position, elapsedTime / moveTime);
            objectsToMove.position = newPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectsToMove.position = location.position;
    }
}
