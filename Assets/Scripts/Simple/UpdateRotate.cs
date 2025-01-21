using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRotate : MonoBehaviour
{
    [SerializeField] Vector3 additiveVector;

    void Update()
    {
        transform.eulerAngles += additiveVector * Time.deltaTime;
        Time.timeScale = 1f;
    }
}
