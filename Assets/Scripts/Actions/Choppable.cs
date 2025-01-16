using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ingredient))]
public class Choppable : MonoBehaviour
{
    Ingredient ingredient;
    private void Awake()
    {
        ingredient = GetComponent<Ingredient>();
    }

    public int requiredChops;
    int chopCount;
    public Mesh choppedMesh;
    public virtual void Chop()
    {
        chopCount++;
        if (chopCount >= requiredChops)
        {
            ingredient.meshFilter.sharedMesh = choppedMesh; //change the mesh to the chopped variant
            ingredient.isChopped = true;
        }
    }
}
