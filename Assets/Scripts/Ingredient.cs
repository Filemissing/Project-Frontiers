using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[System.Serializable]
public class Ingredient : Carryable
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    protected virtual void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public bool isFried;
    public bool isBurnt;
    public bool isChopped;

    public override void InteractLeft()
    {
        if(transform.parent && transform.parent.TryGetComponent<Pan>(out Pan pan) | transform.parent.TryGetComponent<Plate>(out Plate plate))
        {
            return;
        }
        else
        {
            base.InteractLeft();
        }
    }
}