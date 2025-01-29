using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Interact : MonoBehaviour
{
    public bool canInteract;
    public float interactionRange;
    public LayerMask interactionMask;
    private RaycastHit hit;

    [Header("Cursor")]
    public Image cursorImage;
    public Sprite defaultSprite;
    public Sprite interactableSprite;
    //public CursorSprite[] cursorSprites;
    
    void Update()
    {
        if (!canInteract) return;

        ChangeCursor();

        OutLineObject();

        SendMessages();
    }

    void ChangeCursor()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionRange, interactionMask, QueryTriggerInteraction.Collide))
        {
            cursorImage.sprite = interactableSprite;

            //foreach (CursorSprite cursorSprite in cursorSprites)
            //{
            //    if (cursorSprite.CheckComponent(hit.transform.gameObject))
            //    {
            //        cursorImage.sprite = cursorSprite.sprite;
            //        return;
            //    }
            //}
        }
        else cursorImage.sprite = defaultSprite;
    }

    void OutLineObject()
    {
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionRange, interactionMask, QueryTriggerInteraction.Collide))
        {
            if(hit.transform.TryGetComponent<Carryable>(out Carryable carryable))
            {
                carryable.Outline();
            }
        }
    }
    
    void SendMessages()
    {
        if (Input.GetMouseButtonDown(0))// left mouse button
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, interactionMask, QueryTriggerInteraction.Collide))
            {
                hit.transform.SendMessage("InteractLeft", SendMessageOptions.DontRequireReceiver);
            }
        }

        if (Input.GetMouseButtonDown(1))// right mouse button
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, interactionMask, QueryTriggerInteraction.Collide))
            {
                hit.transform.SendMessage("InteractRight", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    //[System.Serializable]
    //public struct CursorSprite
    //{
    //    public Component component;
    //    public Sprite sprite;
    //    public bool CheckComponent(GameObject target)
    //    {
    //        Type type = component.GetType();
    //        if (target.TryGetComponent(type, out Component c))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //}
}