using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FinishedGameUI : MonoBehaviour
{
    public TMP_Text finishedGameTitle;
    public Canvas finishedGameCanvas;
    public Button reloadButton;
    public Button exitButton;

    private void Start()
    {
        reloadButton.onClick.AddListener(ReloadScene);
        exitButton.onClick.AddListener(ExitApplication);
    }

    // This function can be called when the player changes
    public void DisplayFinishedGame()
    {
        finishedGameTitle.text = $"Player 1 je pobedio";
        finishedGameCanvas.enabled = true;
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ExitApplication()
    {
        Debug.Log("Exiting app");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
