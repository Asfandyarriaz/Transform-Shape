using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controls the sway amount i.e: move the object in the direction specified
/// </summary>
public class Sway : MonoBehaviour
{
    [SerializeField] float swayAmount;
    [SerializeField] float swaySpeed;

    //Variables
    float sway;

    private void FixedUpdate()
    {
        sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);
    }
}
