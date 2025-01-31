using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] OrderRequest orderRequest;
    [SerializeField] Image completedOrderImage;
    [SerializeField] RectTransform barRectTransform;

    void LookAtPlayer()
    {
        if (canvas == null || GameManager.instance.player == null)
            return;

        //Vector3 direction = GameManager.instance.player.transform.position - canvas.transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //canvas.transform.rotation = lookRotation;
        transform.LookAt(GameManager.instance.player.transform);
    }

    void UpdateCompletedOrderImage()
    {
        completedOrderImage.sprite = orderRequest.order.sprite;
    }

    void UpdateBar()
    {
        if (!barRectTransform)
            return;

        float timePercentage = orderRequest.timeLeft / orderRequest.maxTime;
        barRectTransform.localScale = new Vector3(timePercentage, 1, 1);
    }

    void Update()
    {
        LookAtPlayer();
        UpdateCompletedOrderImage();
        UpdateBar();
    }
}
