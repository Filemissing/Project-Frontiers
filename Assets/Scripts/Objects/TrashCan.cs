using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] Carryable[] carryableBlacklist;

    void InteractLeft()
    {
        if (GameManager.instance.playerCarry.carryingObject == null)
            return;

        GameObject destroyObject = null;
        bool isBlacklisted = false;

        for (int i = 0; i < carryableBlacklist.Length; i++)
        {
            if (GameManager.instance.playerCarry.carryingObject.GetComponent(carryableBlacklist[i].GetType()) != null)
                isBlacklisted = true;
                if (GameManager.instance.playerCarry.carryingObject.transform.childCount != 0)
                    destroyObject = GameManager.instance.playerCarry.carryingObject.transform.GetChild(0).gameObject;
        }

        if (destroyObject == null && isBlacklisted == false)
        {
            destroyObject = GameManager.instance.playerCarry.carryingObject;
        }

        Destroy(destroyObject);
    }
}
