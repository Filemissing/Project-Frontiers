using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class Recipe : ScriptableObject
{
    public IngredientRequirements[] ingredients;
    public GameObject endResult;
}

[System.Serializable]
public class IngredientRequirements
{
    public Ingredient ingredient;
    public bool isCooked;
    public bool isFried;
    public bool isBurnt;
    public bool isChopped;
}