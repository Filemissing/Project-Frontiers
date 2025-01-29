using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateStack : MonoBehaviour
{
    [SerializeField] GameObject plate;

    void InteractLeft()
    {
        if (!plate)
        {
            Debug.LogWarning("Plate has not been assigned!");
            return;
        }

        // give the player a new plate
        if (GameManager.instance.playerCarry.carryingObject == null)
            GameManager.instance.playerCarry.carryingObject = Instantiate<GameObject>(plate);
    }
}
