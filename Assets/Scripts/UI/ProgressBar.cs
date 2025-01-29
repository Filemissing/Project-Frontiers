using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image progressBar;
    public float progress;
    public Color barColor;
    public Image iconImage;
    public Sprite defaultIcon;
    public Sprite alternateIcon;

    public void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0)); // rotate 180 because UI points at -z

        progressBar.fillAmount = progress;

        progressBar.color = barColor;
    }
}