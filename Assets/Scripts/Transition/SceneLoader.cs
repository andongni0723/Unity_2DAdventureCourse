using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Event")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO afterSceneLoadEventSO;
    public FadeEventSO fadeEventSO;
    
    [Header("Data")]
    public GameSceneSO firstLoadScene;
    private GameSceneSO currentLoadScene;
    
    [Header("Component")]
    public Transform playerTransform;

    [Header("Settings")] 
    public Vector3 firstPosition;
    public float fadeDuration;
    //[Header("Debug")]

    private GameSceneSO locationToGo;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;

    private void Awake()
    {
        // Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        // currentLoadScene = firstLoadScene;
        // currentLoadScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
        NewGame();
    }

    private void Start()
    {
        NewGame();
        //TODO: Main menu
    }

    #region Events

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void OnLoadRequestEvent(GameSceneSO locationToGo, Vector3 positionToGo, bool fadeScreen)
    {
        if(isLoading) return;
        
        isLoading = true;
        this.locationToGo = locationToGo;
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
        locationToGo = firstLoadScene;
        OnLoadRequestEvent(locationToGo, firstPosition, true);
    }
    
    async UniTask UnLoadPreviousScene(CancellationToken ctx)
    {
        if (fadeScreen)
        {
            fadeEventSO.FadeIn(fadeDuration);
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(fadeDuration), cancellationToken: ctx);
        await currentLoadScene.sceneReference.UnLoadScene();
        playerTransform.gameObject.SetActive(false);
        playerTransform.position = positionToGo;
        
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = locationToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
       
        loadingOption.Completed += OnLoadComplete;
        
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        currentLoadScene = locationToGo;

        playerTransform.position = positionToGo;
        playerTransform.gameObject.SetActive(true);
        
        if (fadeScreen)
        {
            
            fadeEventSO.FadeOut(fadeDuration);
        }
        
        isLoading = false;
        afterSceneLoadEventSO.RaiseEvent();
    }
}