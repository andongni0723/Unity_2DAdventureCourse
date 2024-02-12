using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;
    
    /// <summary>
    /// Request to load a scene
    /// </summary>
    /// <param name="sceneToLoad"></param>
    /// <param name="positionToGo"></param>
    /// <param name="fadeScreen"></param>
    public void RaiseLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 positionToGo, bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(sceneToLoad, positionToGo, fadeScreen);
    }
}