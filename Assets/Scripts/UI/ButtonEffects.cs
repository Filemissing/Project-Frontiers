using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour
{
    Image image;
    RectTransform rectTransform;

    [SerializeField] Color enterColor = new Color(.9f, .9f, .9f, 1);
    Color defaultColor;
    [SerializeField] Sprite downSprite;
    Sprite defaultSprite;

    Vector3 defaultPosition;
    Vector3 offsetPosition;
    [SerializeField] Vector3 downOffset = new Vector3(0, -5, 0);

    bool isMouseOnButton = false;


    public void MouseEnter()
    {
        isMouseOnButton = true;

        image.sprite = defaultSprite;
        image.color = enterColor;

        rectTransform.position = defaultPosition;
    }

    public void MouseExit()
    {
        isMouseOnButton = false;

        image.sprite = defaultSprite;
        image.color = defaultColor;

        rectTransform.position = defaultPosition;
    }

    public void MouseDown()
    {
        image.sprite = downSprite;
        image.color = defaultColor;

        rectTransform.position = offsetPosition;
    }

    public void MouseUp()
    {
        image.sprite = defaultSprite;

        if (isMouseOnButton)
            image.color = enterColor;

        rectTransform.position = defaultPosition;
    }


    void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        defaultPosition = rectTransform.position;
        offsetPosition = defaultPosition + downOffset;

        defaultColor = Color.white;
        defaultSprite = image.sprite;
    }
}
