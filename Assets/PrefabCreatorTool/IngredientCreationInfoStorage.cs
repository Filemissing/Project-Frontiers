using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class IngredientCreationInfoStorage : ScriptableObject
{
    public string objectName;
    public Sprite icon;
    public GameObject gameObject;
    public MonoScript createdScript;
    public bool used = true;
}
