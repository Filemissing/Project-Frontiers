using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Carryable : MonoBehaviour
{
    Outline outline;
    int lastOutlineFrame;

    public virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public virtual void Outline()
    {
        outline.enabled = true;
        lastOutlineFrame = Time.frameCount;
    }


    public virtual void Update()
    {
        if(Time.frameCount > lastOutlineFrame)
        {
            outline.enabled = false;
        }
    }

    public virtual void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null)
        {
            GameManager.instance.playerCarry.carryingObject = gameObject;
            gameObject.transform.SetParent(GameManager.instance.player.transform);
        }
    }
}
