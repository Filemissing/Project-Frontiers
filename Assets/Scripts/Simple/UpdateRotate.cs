using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRotate : MonoBehaviour
{
    [SerializeField] Vector3 additiveVector;

    void Update()
    {
        transform.eulerAngles += additiveVector * Time.deltaTime;
        
        // returning from pause menu results in timescale being 0 even though it is explicitly set to 1 when returning
        Time.timeScale = 1f;
    }
}
