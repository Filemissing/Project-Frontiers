using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : Carryable
{
    public Ingredient ingredient;

    public override void InteractLeft()
    {
        if (GameManager.instance.playerCarry.carryingObject == null)
        {
            base.InteractLeft();
        }
        else if (!ingredient && GameManager.instance.TakeCarryingObject<Ingredient>(gameObject, out ingredient))
        {
            // put ingredient on cuttingboard
        }
    }
}
