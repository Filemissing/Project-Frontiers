using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour
{
    public bool canInteract;
    public float interactionRange;
    public LayerMask interactionMask;
    RaycastHit hit;
    
    void Update()
    {
        if (!canInteract) return;

        if (Input.GetMouseButtonDown(0))// left mouse button
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, interactionMask, QueryTriggerInteraction.Collide))
            {
                Debug.Log(hit.GetType());
                Debug.Log(hit);
                Debug.Log(hit.transform.name);
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
