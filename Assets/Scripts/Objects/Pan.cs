using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pan : Carryable
{
    public Ingredient ingredient;
    public Vector3 ingredientPosition;
    public int positionOnStove;
    ProgressBar progressBar;
    Cookable cookable;

    public override void Awake()
    {
        base.Awake();
        progressBar = GetComponentInChildren<ProgressBar>();
    }

    public override void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null)
        {
            if(transform.parent && transform.parent.TryGetComponent<Stove>(out Stove stove))
            {
                stove.panSlots[positionOnStove] = null;
            }
            base.InteractLeft();
        }
        else if(!ingredient && GameManager.instance.TakeCarryingObject<Cookable>(gameObject, out cookable))
        {
            ingredient = cookable.GetComponent<Ingredient>();
            ingredient.transform.position = transform.TransformPoint(ingredientPosition);// put the ingredient in the pan
        }
    }

    public override void Update()
    {
        base .Update();
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        if (ingredient != null)
        {
            progressBar.gameObject.SetActive(true);
            if (cookable.cookTimeCounter <= cookable.cookTime)
            {
                progressBar.iconImage.sprite = progressBar.defaultIcon;
                progressBar.progress = cookable.cookTimeCounter / cookable.cookTime;
                progressBar.barColor = Color.green;
            }
            else
            {
                progressBar.iconImage.sprite = progressBar.alternateIcon;
                progressBar.progress = (cookable.cookTimeCounter - cookable.cookTime) / (cookable.burnTime - cookable.cookTime);
                progressBar.progress = Mathf.Clamp01(progressBar.progress);
                progressBar.barColor = Color.red;
            }
        }
        else progressBar.gameObject.SetActive(false);
    }
}