using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Cinemachine Setting
    [SerializeField] float startFOV, endFOV, duration;
    [SerializeField] AnimationCurve curve;
    CinemachineVirtualCamera cam;

    //Flags
    private bool coroutineAllowed = true;

    private void Start()
    {
        cam = this.gameObject.GetComponent<CinemachineVirtualCamera>();
        cam.m_Lens.FieldOfView = startFOV;
    }
    private void Update()
    {
        if (coroutineAllowed && Input.touchCount > 0) { StartCoroutine(ChangeFOV()); }
    }

    IEnumerator ChangeFOV()
    {
        float time = 0;
        coroutineAllowed = false;
        while(time < duration)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV,endFOV,curve.Evaluate(time/duration));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }    
}
