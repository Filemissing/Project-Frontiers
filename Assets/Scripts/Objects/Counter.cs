using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public Plate[] plateSlots;
    public Transform[] positions;

    public void InteractLeft()
    {
        for (int i = 0; i < plateSlots.Length; i++)
        {
            if (plateSlots[i] == null)
            {
                if (GameManager.instance.TakeCarryingObject<Plate>(gameObject, out Plate plate))
                {
                    plateSlots[i] = plate;
                    plate.transform.position = positions[i].position;
                    plate.transform.rotation = positions[i].rotation;
                    break;
                }
            }
        }
    }
}
