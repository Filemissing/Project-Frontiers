using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public bool doDayNightCycle;
    [Tooltip("All time is in simulated hours")] public float time;
    [Tooltip("All time is in simulated hours")] public float dayStartTime;
    [Tooltip("All time is in simulated hours")] public float dayEndTime;
    float difference;
    float timeSpeed;

    Vector3 originalRotation;

    Light light;
    public Gradient gradient;

    private void Awake()
    {
        if (!doDayNightCycle) return;

        light = GetComponent<Light>();
        originalRotation = transform.rotation.eulerAngles; // store original lightrotation because using transform.rotation.y and z in quaternion.euler stil results in 0

        // convert so that 0 = midnight instead of sun-up
        dayStartTime -= 6;
        dayEndTime -= 6;

        // set the starting values
        transform.rotation = Quaternion.Euler(new Vector3(dayStartTime * 15,  originalRotation.y, originalRotation.z));
        time = dayStartTime;
        light.color = gradient.Evaluate(0);

        // calculate the speed of time passing
        difference = dayEndTime - dayStartTime;
        float dayLength = GameManager.instance.maxDayTime;
        timeSpeed = difference / dayLength;
    }

    void FixedUpdate()
    {
        if (!doDayNightCycle) return;
        if (!GameManager.instance.isDayCyling) return;

        time += timeSpeed * Time.deltaTime; // use Time.deltaTime to convert from seconds to frame time
        transform.rotation = Quaternion.Euler(new Vector3(time * 15, originalRotation.y, originalRotation.z));

        light.color = gradient.Evaluate(time / difference);
    }
}