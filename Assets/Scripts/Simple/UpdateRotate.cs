using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRotate : MonoBehaviour
{
    [SerializeField] Vector3 additiveVector;

    void FixedUpdate()
    {
        transform.eulerAngles += additiveVector;
        Time.timeScale = 1f;
    }
}
