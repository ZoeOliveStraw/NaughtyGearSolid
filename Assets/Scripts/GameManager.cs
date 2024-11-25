using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Enum_GameStates startingState;
    [SerializeField] private Camera loadingCamera;
    [SerializeField] private float loadScreenFadeTime = 2;
    
    private string currentSceneName;
    public static GameManager Instance;
    [SerializeField] public GameObject loadingScreenGo;
    [SerializeField] public UI_LoadingScreen loadingScreenUI;
    [HideInInspector] public LevelInfo currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EnterState(startingState);
    }

    public void EnterState(Enum_GameStates state, bool fadeIn = true)
    {
        switch (state)
        {
            case Enum_GameStates.MainMenu:
                LoadMainMenuState();
                break;
            case Enum_GameStates.MapScreen:
                LoadMapState(fadeIn);
                break;
            case Enum_GameStates.Gameplay:
                LoadGameplayState();
                break;
            case Enum_GameStates.GameOver:
                LoadGameoverState();
                break;
        }
    }
    
    //Scene loading system
    public IEnumerator LoadScene(string sceneName, bool fadeIn = true)
    {
        SetNewMainCamera(loadingCamera);
        loadingScreenGo.SetActive(true);
        if (fadeIn)
        {
            yield return new WaitForSeconds(loadingScreenUI.LoadFadeIn(loadScreenFadeTime));
        }
        
        if (!string.IsNullOrEmpty(currentSceneName))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentSceneName);
            while (asyncUnload != null && !asyncUnload.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        
        currentSceneName = sceneName;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (asyncLoad != null && !asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(loadingScreenUI.LoadFadeOut(loadScreenFadeTime));
        loadingScreenGo.SetActive(false);
    }

    public float LoadingScreenFadeIn(float duration)
    {
        loadingScreenGo.SetActive(true);
        loadingScreenUI.LoadFadeIn(duration);
        return duration;
    }
    
    public float LoadingScreenFadeOut(float duration)
    {
        loadingScreenGo.SetActive(true);
        loadingScreenUI.LoadFadeOut(duration);
        loadingScreenGo.SetActive(false);
        return duration;
    }

    public void SetNewMainCamera(Camera newMainCamera)
    {
        if (Camera.main != null)
        {
            Camera.main.gameObject.SetActive(false);
        }
        newMainCamera.gameObject.SetActive(true);
    }

    public void LoadMainMenuState()
    {
        StartCoroutine(LoadScene("MainMenu"));
    }

    public void LoadMapState(bool fadeIn = true)
    {
        StartCoroutine(LoadScene("MapScreen"));
    }

    public void LoadGameplayState()
    {
        StartCoroutine(LoadScene("Gameplay"));
    }
    
    public void LoadGameoverState()
    {
        
    }
}
