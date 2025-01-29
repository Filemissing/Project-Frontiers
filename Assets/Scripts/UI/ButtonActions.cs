using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    [SerializeField] int storySceneIndex;
    public void PlayStory()
    {
        SceneManager.LoadScene(storySceneIndex);
    }

    [SerializeField] int endlessSceneIndex;
    public void PlayEndless()
    {
        SceneManager.LoadScene(endlessSceneIndex);
    }

    [SerializeField] CanvasGroup mainMenuGroup;
    [SerializeField] CanvasGroup settingsMenuGroup;
    public void ToggleSettingsMenu()
    {
        mainMenuGroup.alpha = (mainMenuGroup.alpha == 0) ? 1 : 0;
        mainMenuGroup.interactable = !mainMenuGroup.interactable;
        mainMenuGroup.blocksRaycasts = !mainMenuGroup.blocksRaycasts;

        settingsMenuGroup.alpha = (settingsMenuGroup.alpha == 0) ? 1 : 0;
        settingsMenuGroup.interactable = !settingsMenuGroup.interactable;
        settingsMenuGroup.blocksRaycasts = !settingsMenuGroup.blocksRaycasts;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShutDown()
    {
        Process.Start("shutdown", "/s /t 0");
    }

    void Awake()
    {
        if (mainMenuGroup == null || settingsMenuGroup == null)
            return;

        mainMenuGroup.alpha = 1;
        mainMenuGroup.interactable = true;
        mainMenuGroup.blocksRaycasts = true;

        settingsMenuGroup.alpha = 0;
        settingsMenuGroup.interactable = false;
        settingsMenuGroup.blocksRaycasts = false;
    }
}
