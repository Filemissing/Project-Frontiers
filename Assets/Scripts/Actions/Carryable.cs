using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carryable : MonoBehaviour
{
    public virtual void InteractLeft()
    {
        if(GameManager.instance.playerCarry.carryingObject == null)
        {
            GameManager.instance.playerCarry.carryingObject = gameObject;
        }
    }
}
