using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[System.Serializable]
public class Ingredient : Carryable
{
    public Sprite icon;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    protected virtual void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public bool isCooked;
    public bool isFried;
    public bool isBurnt;
    public bool isChopped;

    public override void InteractLeft()
    {
        if(transform.parent)
        {
            if (transform.parent.TryGetComponent<Pan>(out Pan pan))
            {
                return; // don't allow pickup from pan
            }
            else if(transform.parent.TryGetComponent<Plate>(out Plate plate))
            {
                return; // don't allow pickup from plate 
            }
            else if(transform.parent.TryGetComponent<FryBasket>(out FryBasket fryBasket))
            {
                return; // don't allow pickup...
            }
            else if(transform.parent.TryGetComponent<CuttingBoard>(out CuttingBoard cuttingBoard))
            {
                base.InteractLeft();
                cuttingBoard.ingredient = null;
            }
            else
            {
                base.InteractLeft();
            }
        }
        else
        {
            base.InteractLeft();
        }
    }

    public void InteractRight()
    {
        if(transform.parent && transform.parent.TryGetComponent<CuttingBoard>(out CuttingBoard cuttingBoard))
        {
            if(GameManager.instance.playerCarry.carryingObject && GameManager.instance.playerCarry.carryingObject.TryGetComponent<Knife>(out Knife knife))
            {
                SendMessage("Chop");
            }
        }
    }
}