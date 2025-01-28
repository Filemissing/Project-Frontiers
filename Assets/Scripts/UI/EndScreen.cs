using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public bool isVisible;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] CanvasGroup gameplayCanvasGroup;
    [SerializeField] TMP_Text textLabel;
    [SerializeField] float transitionTime = .5f;

    [Header("Scores")]
    [SerializeField] TMP_Text timeLabel;
    [SerializeField] TMP_Text customersLabel;
    [SerializeField] CanvasGroup timeTextLabelCanvasGroup;
    [SerializeField] CanvasGroup customersTextLabelCanvasGroup;

    [Header("Messages")]
    [SerializeField] Message[] messages;

    bool previousIsVisible;
    float timeStart;


    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    string ConvertSecondsToTimeString(float time)
    {
        string stringValue;

        int minutes = 0;
        int seconds = 0;

        while (time >= 60)
        {
            time -= 60;
            minutes++;
        }
        seconds = (int)time;

        string string1 = "";
        string string2 = "";


        if (minutes < 10)
            string1 = "0" + minutes.ToString();
        else
            string1 = minutes.ToString();
        
        if (seconds < 10)
            string2 = "0" + seconds.ToString();
        else
            string2 = seconds.ToString();
        

        stringValue = string1 + ":" + string2;
        

        return stringValue;
    }

    void UpdateScores()
    {
        if (!GameManager.instance.isEndlessMode) // Checks if in endless mode
            return;
        
        if (timeTextLabelCanvasGroup.alpha == 1) // Makes method only run once
            return;

        timeTextLabelCanvasGroup.alpha = 1;
        customersTextLabelCanvasGroup.alpha = 1;

        float timeSurvived = Time.time - timeStart;
        int customersServed = 0;

        for (int i = 0; i < GameManager.instance.ratings.Count; i++) // Gets all customers served
        {
            float rating = GameManager.instance.ratings[i];

            if (rating > 1)
                customersServed++;
        }

        timeLabel.text = ConvertSecondsToTimeString(timeSurvived);
        customersLabel.text = customersServed.ToString();
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


                for (int i = 0; i < messages.Length; i++) // Says the correct message
                {
                    Message message = messages[i];

                    if (message.rating == (int)GameManager.instance.rating)
                        textLabel.text = message.text;
                }

                UpdateScores();
            }
            else
                canvasGroup.alpha = 0;
        }

        previousIsVisible = isVisible;
    }

    void Awake()
    {
        timeStart = Time.time;
    }
}
