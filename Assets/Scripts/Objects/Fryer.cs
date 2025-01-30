using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Fryer : MonoBehaviour
{
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

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
        int notFryingCount = 0;
        for (int i = 0; i < basketSlots.Length; i++)
        {
            if (basketSlots[i] != null && basketSlots[i].ingredient != null)
            {
                if(!source.isPlaying) source.Play();
                basketSlots[i].ingredient.SendMessage("Fry");
            }
            else
            {
                notFryingCount++;
            }
        }

        if(notFryingCount == basketSlots.Length)
        {
            if(source.isPlaying) source.Stop();
        }
    }
}
