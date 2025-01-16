using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public Ingredient ingredient;

    public void InteractLeft()
    {
        if (!ingredient && GameManager.instance.TakeCarryingObject<Ingredient>(gameObject, out ingredient))
        {
            // put ingredient on cuttingboard
        }
    }
}
