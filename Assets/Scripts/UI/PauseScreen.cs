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
        if (Input.GetKeyDown(pauseKey)) isPaused = !isPaused;

        if (isPaused != lastPauseState)
        {
            if (isPaused) GameManager.instance.EnterUIMode();
            else GameManager.instance.ExitUIMode(); 
        }

        Time.timeScale = isPaused ? 0f : 1f;
        GameManager.instance.player.SetActive(!isPaused);

        canvas.enabled = isPaused;
        gamePlayCanvas.enabled = !isPaused;

        lastPauseState = isPaused;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene(0);
    }
}