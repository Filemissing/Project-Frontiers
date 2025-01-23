using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Tooltip("All time is in simulated hours")] public float time;
    [Tooltip("All time is in simulated hours")] public float dayStartTime;
    [Tooltip("All time is in simulated hours")] public float dayEndTime;
    float difference;
    float timeSpeed;

    Light light;
    public Gradient gradient;

    private void Awake()
    {
        light = GetComponent<Light>();

        // convert so that 0 = midnight instead of sun-up
        dayStartTime -= 6;
        dayEndTime -= 6;

        // set the starting values
        transform.rotation = Quaternion.Euler(dayStartTime * 15,  transform.rotation.y, transform.rotation.z);
        time = dayStartTime;
        light.color = gradient.Evaluate(0);

        // calculate the speed of time passing
        difference = dayEndTime - dayStartTime;
        float dayLength = GameManager.instance.maxDayTime;
        timeSpeed = difference / dayLength;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isDayCyling) return;
        time += timeSpeed * Time.deltaTime; // use Time.deltaTime to convert from seconds to frame time
        transform.rotation = Quaternion.Euler(time * 15, transform.rotation.y, transform.rotation.z);

        light.color = gradient.Evaluate(time / difference);
    }
}