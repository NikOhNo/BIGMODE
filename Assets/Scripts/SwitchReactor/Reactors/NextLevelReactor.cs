using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelReactor : ReactorBase
{
    SceneLoader loader;

    private void Awake()
    {
        loader = GetComponent<SceneLoader>();
    }
    public override void EndReaction()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformReaction()
    {
        
        loader.LoadNextScene();
    }
}
