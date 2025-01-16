using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    void InteractLeft()
    {
        Destroy(GameManager.instance.playerCarry.carryingObject);
    }
}
