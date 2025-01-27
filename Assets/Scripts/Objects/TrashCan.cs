using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] Carryable[] carryableBlacklist; // blacklist items that cannot be thrown away

    void InteractLeft()
    {
        if (GameManager.instance.playerCarry.carryingObject == null)
            return;

        GameObject destroyObject = null;
        bool isBlacklisted = false;

        for (int i = 0; i < carryableBlacklist.Length; i++)
        {
            if (GameManager.instance.playerCarry.carryingObject.GetComponent(carryableBlacklist[i].GetType()) != null)
            {
                isBlacklisted = true;
                if (GameManager.instance.playerCarry.carryingObject.transform.childCount > 1)
                    destroyObject = GameManager.instance.playerCarry.carryingObject.transform.GetChild(1).gameObject; // get child 1 so you don't throw away the progress bars
            }
        }

        if (destroyObject == null && isBlacklisted == false)
        {
            destroyObject = GameManager.instance.playerCarry.carryingObject;
        }

        Destroy(destroyObject);
    }
}
