using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Carryable
{
    public Ingredient ingredient;
    public Vector3 ingredientPosition;
    public int positionOnStove;
    
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
        else if(!ingredient && GameManager.instance.TakeCarryingObject<Cookable>(gameObject, out Cookable returnComponent))
        {
            ingredient = returnComponent.GetComponent<Ingredient>();
            ingredient.transform.position = transform.TransformPoint(ingredientPosition);// put the ingredient in the pan
        }
    }
}