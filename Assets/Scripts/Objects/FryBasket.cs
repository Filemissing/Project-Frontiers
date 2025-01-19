using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryBasket : Carryable
{
    public Ingredient ingredient;
    public Vector3 ingredientPosition;
    public int positionOnFryer;
    
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
        else if(!ingredient && GameManager.instance.TakeCarryingObject<Fryable>(gameObject, out Fryable returnComponent))
        {
            ingredient = returnComponent.GetComponent<Ingredient>();
            ingredient.transform.position = transform.TransformPoint(ingredientPosition);// put the ingredient in the basket
        }
    }
}