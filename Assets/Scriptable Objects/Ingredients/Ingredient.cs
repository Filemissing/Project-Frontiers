using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    public virtual void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    [Header("Chopping")]
    public bool canChop;
    public int requiredChops;
    int chopCount;
    public Mesh choppedMesh;
    public virtual void Chop()
    {
        if(!canChop) return;

        chopCount++;
        if(chopCount >= requiredChops ) meshFilter.sharedMesh = choppedMesh; //change the mesh to the chopped variant
    }

    [Header("Frying")]
    public bool canFry;
    public int fryTime;
    public int burnTime;
    float fryTimeCounter;
    public Material friedMaterial;
    public Material burntMaterial;
    public virtual void Fry()
    {
        if(!canFry) return;

        fryTimeCounter += Time.deltaTime;

        if(fryTimeCounter >= fryTime) meshRenderer.material = friedMaterial; //change the material when fried
        if(fryTimeCounter >= burnTime) meshRenderer.material = burntMaterial; //change the material when burnt
    }
}