using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class Knife : Carryable
{
    public override void InteractLeft()
    {
        if(transform.parent && transform.parent.TryGetComponent<KnifeHolder>(out KnifeHolder holder))
        {
            holder.knife = null;
        }
        base.InteractLeft();
    }
}
