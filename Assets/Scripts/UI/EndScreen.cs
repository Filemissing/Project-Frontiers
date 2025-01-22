using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public bool isVisible;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup gameplayCanvasGroup;
    [SerializeField] float transitionTime = .5f;

    bool previousIsVisible;

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (GameManager.instance.dayTimeLeft == 0)
            isVisible = true;
        else
            isVisible = false;

        if (isVisible != previousIsVisible) // Checks if isVisible changed
        {
            if (isVisible)
            {
                Time.timeScale = 0;
                canvasGroup.DOFade(1, transitionTime).SetUpdate(true);
                gameplayCanvasGroup.DOFade(0, transitionTime).SetUpdate(true);
                GameManager.instance.EnterUIMode();
            }
            else
                canvasGroup.alpha = 0;
        }

        previousIsVisible = isVisible;
    }
}
