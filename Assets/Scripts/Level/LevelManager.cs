using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [HideInInspector] public LevelInfo currentLevelInfo;
    private string _currentlyLoadedStage;
    private int _currentStageIndex;

    public static LevelManager Instance;
    
    private void Start()
    {
        Debug.LogWarning("LevelManager.Start() called!");
        Instance = this;
        
        if (GameManager.Instance != null && GameManager.Instance.currentLevel != null)
        {
            currentLevelInfo = GameManager.Instance.currentLevel;
            LoadFirstStage();
        }
        else
        {
            GameManager.Instance.EnterState(Enum_GameStates.MapScreen);
        }
    }

    private void LoadFirstStage()
    {
        string firstStageName = currentLevelInfo.levelStages[0].levelSceneName;
        _currentStageIndex = 0;
        SceneManager.LoadSceneAsync(firstStageName, LoadSceneMode.Additive);
        _currentlyLoadedStage = firstStageName;
    }

    public void TryLoadNextStage()
    {
        if (_currentStageIndex >= currentLevelInfo.levelStages.Count - 1)
        {
            StartCoroutine(LoadLevelCompletionScene());
        }
        else
        {
            _currentStageIndex++;
            LoadStage(currentLevelInfo.levelStages[_currentStageIndex].levelSceneName);
        }
    }

    private void LoadStage(string stageName)
    {
        StartCoroutine(LoadStageCoroutine(stageName));
    }

    private IEnumerator LoadStageCoroutine(string stageName)
    {
        GameObject loadingScreenGo = GameManager.Instance.loadingScreenGo;
        UI_LoadingScreen loadingScreenUI = GameManager.Instance.loadingScreenUI;
        
        loadingScreenGo.SetActive(true);
        yield return new WaitForSeconds(loadingScreenUI.LoadFadeIn(1));
        
        if (!string.IsNullOrEmpty(_currentlyLoadedStage))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_currentlyLoadedStage);
            while (asyncUnload != null && !asyncUnload.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        
        _currentlyLoadedStage = stageName;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(stageName, LoadSceneMode.Additive);
        while (asyncLoad != null && !asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(loadingScreenUI.LoadFadeOut(1));
        loadingScreenGo.SetActive(false);
    }

    private IEnumerator LoadLevelCompletionScene()
    {
        GameObject loadingScreenGo = GameManager.Instance.loadingScreenGo;
        UI_LoadingScreen loadingScreenUI = GameManager.Instance.loadingScreenUI;
        
        loadingScreenGo.SetActive(true);
        yield return new WaitForSeconds(loadingScreenUI.LoadFadeIn(1));
        
        if (!string.IsNullOrEmpty(_currentlyLoadedStage))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_currentlyLoadedStage);
            while (asyncUnload != null && !asyncUnload.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        GameManager.Instance.EnterState(Enum_GameStates.MapScreen, false);
    }

    private void LoadGameOver()
    {
        GameManager.Instance.EnterState(Enum_GameStates.MapScreen);
    }

    private void ResetLevel()
    {
        GameManager.Instance.EnterState(Enum_GameStates.Gameplay);
    }
}
