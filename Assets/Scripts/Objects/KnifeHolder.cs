using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KnifeHolder : MonoBehaviour
{
    public Knife knife;
    public Transform position;

    public void InteractLeft()
    {
        if(knife == null)
        {
            if (GameManager.instance.TakeCarryingObject<Knife>(gameObject, out Knife knife))
            {
                this.knife = knife;
                knife.transform.SetPositionAndRotation(position.position, position.rotation);
            }
        } 
    }
}
