using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryBasket : Carryable
{
    public Ingredient ingredient;
    public Vector3 ingredientPosition;
    public int positionOnFryer;
    ProgressBar progressBar;
    Fryable fryable;

    public override void Awake()
    {
        base.Awake();
        progressBar = GetComponentInChildren<ProgressBar>();
    }

    public override void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null)
        {
            if(transform.parent && transform.parent.TryGetComponent<Fryer>(out Fryer fryer))
            {
                fryer.basketSlots[positionOnFryer] = null;
            }
            base.InteractLeft();
        }
        else if(!ingredient && GameManager.instance.TakeCarryingObject<Fryable>(gameObject, out fryable))
        {
            ingredient = fryable.GetComponent<Ingredient>();
            ingredient.transform.position = transform.TransformPoint(ingredientPosition);// put the ingredient in the basket
        }
    }

    public override void Update()
    {
        base.Update();
        UpdateProgressBar();
    }

    void UpdateProgressBar()
    {
        if (ingredient != null)
        {
            progressBar.gameObject.SetActive(true);
            if (fryable.fryTimeCounter <= fryable.fryTime)
            {
                progressBar.iconImage.sprite = progressBar.defaultIcon;
                progressBar.progress = fryable.fryTimeCounter / fryable.fryTime;
                progressBar.barColor = Color.green;
            }
            else
            {
                progressBar.iconImage.sprite = progressBar.alternateIcon;
                progressBar.progress = (fryable.fryTimeCounter - fryable.fryTime) / (fryable.burnTime - fryable.fryTime);
                progressBar.progress = Mathf.Clamp01(progressBar.progress);
                progressBar.barColor = Color.red;
            }
        }
        else progressBar.gameObject.SetActive(false);
    }
}