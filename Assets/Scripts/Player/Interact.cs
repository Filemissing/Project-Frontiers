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
    public Sprite highlightSprite;
    
    void Update()
    {
        if (!canInteract) return;

        ChangeCursor();

        SendMessages();
    }

    void ChangeCursor()
    {
        bool highlight = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, interactionRange, interactionMask, QueryTriggerInteraction.Collide);
        cursorImage.sprite = highlight ? highlightSprite : defaultSprite;
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
}