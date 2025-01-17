using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] Carryable[] carryableBlacklist;

    void InteractLeft()
    {
        GameObject destroyObject = null;

        for (int i = 0; i < carryableBlacklist.Length; i++)
        {
            if (GameManager.instance.playerCarry.carryingObject.GetComponent(carryableBlacklist[i].GetType()) != null)
                if (GameManager.instance.playerCarry.carryingObject.transform.childCount != 0)
                    destroyObject = GameManager.instance.playerCarry.carryingObject.transform.GetChild(0).gameObject;
        }

        Destroy(destroyObject);
    }
}
