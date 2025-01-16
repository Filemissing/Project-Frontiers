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
            base.InteractLeft();
            if(transform.parent && transform.parent.TryGetComponent<Stove>(out Stove stove))
            {
                stove.pan = null;
            }
        }
        else if(!ingredient && GameManager.instance.playerCarry.carryingObject.TryGetComponent<Ingredient>(out ingredient))
        {
            GameManager.instance.playerCarry.carryingObject = null;
            ingredient.transform.SetParent(transform);
            ingredient.transform.position = transform.position;

        }
    }

    
}