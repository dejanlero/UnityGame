using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public Canvas mainMenuCanvas;
    public Canvas currentPlayerCanvas;
    [HideInInspector] public bool isStartMenuActive = true;

    private void Start() {
        currentPlayerCanvas.enabled = false;
    }
    public void SetMainMenuActive(bool isActive)
    {
        mainMenuCanvas.enabled = isActive;
        isStartMenuActive = isActive;
        currentPlayerCanvas.enabled = !isActive;
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


