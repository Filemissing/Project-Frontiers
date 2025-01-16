using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : Carryable
{
    public Ingredient ingredient;
    
    public override void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null)
        {
            if(transform.parent && transform.parent.TryGetComponent<Stove>(out Stove stove))
            {
                stove.pan = null;
            }
            base.InteractLeft();
        }
        else if(!ingredient && GameManager.instance.TakeCarryingObject<Ingredient>(gameObject, out ingredient))
        {
            // put the ingredient in the pan
        }
    }
}