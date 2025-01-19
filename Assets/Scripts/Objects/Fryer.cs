using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fryer : MonoBehaviour
{
    public FryBasket[] basketSlots;
    public Transform[] positions;
    
    void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null) return;

        if(GameManager.instance.TakeCarryingObject<FryBasket>(gameObject, out FryBasket basket))
        {
            for (int i = 0; i < basketSlots.Length; i++)
            {
                if (basketSlots[i] == null)
                {
                    basketSlots[i] = basket;
                    basket.positionOnFryer = i;
                    basket.transform.position = positions[i].position;
                    basket.transform.rotation = positions[i].rotation;
                    break;
                }
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < basketSlots.Length; i++)
        {
            if (basketSlots[i] != null && basketSlots[i].ingredient != null)
            {
                basketSlots[i].ingredient.SendMessage("Fry");
            }
        }
    }
}
