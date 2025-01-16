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

    void LookAtPlayer()
    {
        if (canvas == null || GameManager.instance.player == null)
            return;

        Vector3 direction = GameManager.instance.player.transform.position - canvas.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        canvas.transform.rotation = lookRotation;
    }

    void UpdateCompletedOrderImage()
    {
        completedOrderImage.sprite = orderRequest.order.sprite;
    }

    void Update()
    {
        LookAtPlayer();
        UpdateCompletedOrderImage();
    }
}
