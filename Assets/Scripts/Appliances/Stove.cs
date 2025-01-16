using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public Pan pan;
    
    void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null) return;

        if(GameManager.instance.playerCarry.carryingObject.TryGetComponent<Pan>(out pan))
        {
            GameManager.instance.playerCarry.carryingObject = null;
            pan.transform.SetParent(transform);
            pan.transform.position = transform.position;
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
