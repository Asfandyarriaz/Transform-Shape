using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineVibrations : MonoBehaviour
{
    [SerializeField] float vibrationIntensity = 0.1f;

    private void Update()
    {
        float vibration = Mathf.Sin(Time.time) * vibrationIntensity;
        transform.Translate(Vector3.up * vibration * Time.deltaTime);
    }
}
