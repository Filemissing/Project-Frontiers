using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Stove : MonoBehaviour
{
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public Pan[] panSlots;
    public Transform[] positions;
    
    void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null) return;

        // put the pan on the stove
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
        int notCookingSlots = 0;
        for (int i = 0; i < panSlots.Length; i++)
        {
            if (panSlots[i] != null && panSlots[i].ingredient != null)
            {
                if(!source.isPlaying) source.Play();
                panSlots[i].ingredient.SendMessage("Cook"); // cook the ingredient in the pan
            }
            else
            {
                notCookingSlots++;
            }
        }

        if(notCookingSlots == panSlots.Length)
        {
            if(source.isPlaying) source.Stop();
        }
    }
}
