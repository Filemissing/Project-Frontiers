using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    public Ingredient ingredient;
    public Vector3 ingredientPosition;

    public void InteractLeft()
    {
        if (!ingredient && GameManager.instance.TakeCarryingObject<Choppable>(gameObject, out Choppable returnComponent))
        {
            ingredient = returnComponent.GetComponent<Ingredient>();
            ingredient.transform.position = transform.TransformPoint(ingredientPosition); // put ingredient on cuttingboard
        }
    }
}
