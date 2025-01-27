using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public Ingredient ingredient;
    public Vector3 ingredientPosition;
    ProgressBar progressBar;
    Choppable choppable;

    private void Awake()
    {
        progressBar = GetComponentInChildren<ProgressBar>();
    }

    public void InteractLeft()
    {
        if (!ingredient && GameManager.instance.TakeCarryingObject<Choppable>(gameObject, out choppable))
        {
            ingredient = choppable.GetComponent<Ingredient>();
            ingredient.transform.position = transform.TransformPoint(ingredientPosition); // put ingredient on cuttingboard
        }
    }

    public void Update()
    {
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        if (ingredient != null)
        {
            progressBar.gameObject.SetActive(true);
            progressBar.iconImage.sprite = progressBar.defaultIcon;
            progressBar.progress = choppable.chopCount / choppable.requiredChops;
            progressBar.progress = Mathf.Clamp01(progressBar.progress);
            progressBar.barColor = Color.green;
        }
        else progressBar.gameObject.SetActive(false);
    }
}
