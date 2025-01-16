using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public Pan pan;
    
    void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null) return;

        if(GameManager.instance.TakeCarryingObject<Pan>(gameObject, out pan))
        {
            // place the pan on the stove
        }
    }

    void Update()
    {
        if(pan != null && pan.ingredient != null)
        {
            pan.ingredient.SendMessage("Fry");
        }
    }
}
