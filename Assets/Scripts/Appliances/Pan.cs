using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pan : MonoBehaviour
{
    public Ingredient ingredient;
    
    public void InteractLeft()
    {
        if(!ingredient && GameManager.instance.playerCarry.carryingObject.TryGetComponent<Ingredient>(out ingredient))
        {
            GameManager.instance.playerCarry.carryingObject = null;
            ingredient.transform.SetParent(transform);
            ingredient.transform.position = Vector3.zero;
        }
    }
}