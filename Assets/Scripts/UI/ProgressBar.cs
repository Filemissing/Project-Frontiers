using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public RectTransform progressBar;
    Image progressBarImage;
    public float progress;
    public Color barColor;
    public Image iconImage;
    public Sprite defaultIcon;
    public Sprite alternateIcon;

    private void Awake()
    {
        progressBarImage = progressBar.GetComponent<Image>();
    }

    public void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0));

        progressBar.localScale = new Vector3(progress, 1, 1);

        progressBarImage.color = barColor;
    }
}