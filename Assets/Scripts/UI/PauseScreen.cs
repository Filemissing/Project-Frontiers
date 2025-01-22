using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public KeyCode pauseKey;
    public bool isPaused {  get;  set; }
    bool lastPauseState;
    public Canvas gamePlayCanvas;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (GameManager.instance.dayTimeLeft != 0) // Checks if day hasen't ended.
            if (Input.GetKeyDown(pauseKey)) isPaused = !isPaused;
        else
            //isPaused = false;

        if (isPaused != lastPauseState)
        {
            if (isPaused)
            {
                Time.timeScale = 0;
                GameManager.instance.EnterUIMode();
            }
            else
            {
                Time.timeScale = 1;
                GameManager.instance.ExitUIMode();
            }
        }

        //Time.timeScale = isPaused ? 0f : 1f;
        GameManager.instance.player.SetActive(!isPaused);

        canvas.enabled = isPaused;
        gamePlayCanvas.enabled = !isPaused;

        lastPauseState = isPaused;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}