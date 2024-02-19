using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    [Header("Event")]
    public SceneLoadEventSO loadEventSO;
    public SceneLoadEventSO unloadEventSO;
    public VoidEventSO afterSceneLoadEventSO;
    public FadeEventSO fadeEventSO;
    public VoidEventSO newGameEventSO;
    public VoidEventSO backToMenuEventSO;
    
    [Header("Data")]
    public GameSceneSO menuScene;
    public GameSceneSO firstLoadScene;
    private GameSceneSO currentLoadScene;
    
    [Header("Component")]
    public Transform playerTransform;

    [Header("Settings")] 
    public Vector3 menuPosition;
    public Vector3 firstPosition;
    public float fadeDuration;
    
    //[Header("Debug")]
    private GameSceneSO sceneToGo;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;

    private void Start()
    {
        Application.targetFrameRate = 300;
        // Main menu
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
    }

    #region Events

    private void OnEnable()
    {
        
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEventSO.OnEventRaised += NewGame;
        backToMenuEventSO.OnEventRaised += OnBackToMenuEvent;
        
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEventSO.OnEventRaised += NewGame;
        backToMenuEventSO.OnEventRaised -= OnBackToMenuEvent;
        
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackToMenuEvent()
    {
        sceneToGo = menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, menuPosition, true);
    }

    private void OnLoadRequestEvent(GameSceneSO sceneToGo, Vector3 positionToGo, bool fadeScreen)
    {
        if(isLoading) return;
        
        isLoading = true;
        this.sceneToGo = sceneToGo;
        this.positionToGo = positionToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene != null)
        {
            var ctsInfo = TaskSingol.CreatCts();
            UnLoadPreviousScene(ctsInfo.cts.Token); 
        }
        else
        {
            LoadNewScene();
        }
    }
    #endregion

    private void NewGame()
    {
        sceneToGo = firstLoadScene;
        // OnLoadRequestEvent(locationToGo, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, firstPosition, true);
    }
    
    async UniTask UnLoadPreviousScene(CancellationToken ctx)
    {
        if (fadeScreen)
        {
            fadeEventSO.FadeIn(fadeDuration);
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(fadeDuration), cancellationToken: ctx);
        
        unloadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, fadeScreen);
        
        await currentLoadScene.sceneReference.UnLoadScene();
        playerTransform.gameObject.SetActive(false);
        playerTransform.position = positionToGo;
        
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
       
        loadingOption.Completed += OnLoadComplete;
        
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadScene = sceneToGo;

        playerTransform.position = positionToGo;
        playerTransform.gameObject.SetActive(true);
        
        if (fadeScreen)
        {
            
            fadeEventSO.FadeOut(fadeDuration);
        }
        
        isLoading = false;
        
        if(currentLoadScene.sceneType != SceneType.Menu)
            afterSceneLoadEventSO.RaiseEvent();
        
        // DataManager.instance.Save();
        // DataManager.instance.Load();
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTransform.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionToGo = data.characterPosDict[playerID]; 
            sceneToGo = data.GetSavedScene();
            
            // DataManager.instance.Save();
            OnLoadRequestEvent(sceneToGo, positionToGo, true);
        }
    }
}