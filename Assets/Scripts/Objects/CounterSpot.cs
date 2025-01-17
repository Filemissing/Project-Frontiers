using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSpot : MonoBehaviour
{
    public void InteractLeft()
    {
        if (transform.childCount == 0)
        {
            if (GameManager.instance.TakeCarryingObject<Transform>(gameObject, out Transform transformCarryingObject))
            {
                Vector3 newPosition = transform.position;
                if (transformCarryingObject.gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
                    newPosition.y += renderer.bounds.size.y / 2;

                transformCarryingObject.position = newPosition;
                transformCarryingObject.rotation = transform.rotation;
            }
        }
    }
}
