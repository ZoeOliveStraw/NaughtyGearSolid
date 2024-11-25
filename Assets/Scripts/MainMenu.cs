using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.SetNewMainCamera(sceneCamera);
    }

    public void Click_PlayGame()
    {
        GameManager.Instance.EnterState(Enum_GameStates.MapScreen);
    }
}
