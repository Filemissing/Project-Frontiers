using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carry : MonoBehaviour
{
    public GameObject carryingObject;
    public Vector3 offset;
    public bool confineToCarryPosition;
    void Update()
    {
        if(confineToCarryPosition && carryingObject != null)
        {
            carryingObject.transform.position = transform.position + transform.TransformDirection(offset);
            carryingObject.transform.rotation = transform.rotation;
        }
    }
}
