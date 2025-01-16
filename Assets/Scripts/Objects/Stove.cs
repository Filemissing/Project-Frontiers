using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public Pan[] panSlots;
    public Transform[] positions;
    
    void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null) return;

        if(GameManager.instance.TakeCarryingObject<Pan>(gameObject, out Pan pan))
        {
            for (int i = 0; i < panSlots.Length; i++)
            {
                if (panSlots[i] == null)
                {
                    panSlots[i] = pan;
                    pan.positionOnStove = i;
                    pan.transform.position = positions[i].position;
                    pan.transform.rotation = positions[i].rotation;
                    break;
                }
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < panSlots.Length; i++)
        {
            if (panSlots[i] != null && panSlots[i].ingredient != null)
            {
                panSlots[i].ingredient.SendMessage("Fry");
            }
        }
    }
}
