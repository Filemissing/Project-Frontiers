using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rating : MonoBehaviour
{
    [SerializeField] Image barImage;

    void Update()
    {
        float currentRating = GameManager.instance.rating;
        if (float.IsNaN(currentRating))
            currentRating = .001f;
        currentRating = Mathf.Clamp(currentRating, .001f, 5f);

        float fillPercentage = currentRating / 5;
        barImage.fillAmount = fillPercentage;
    }
}
