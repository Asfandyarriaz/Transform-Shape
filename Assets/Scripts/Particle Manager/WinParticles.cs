using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinParticles : MonoBehaviour
{
    [Header("Win Particles")]
    [SerializeField] ParticleManager particleManagerScript;
    [SerializeField] GameObject particlePlaceholder1;
    [SerializeField] GameObject particlePlaceholder2;


    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.Start)
        {
            if(gameObject.activeSelf)
            {
                SetParticlesPosition();
            }
        }
        if(state == GameManager.GameState.ProgressionScreen || state == GameManager.GameState.Win || state == GameManager.GameState.Cash)
        {
            ActivateParticles(true);
        }
    }

    //Set Active Particles False
    private void Start()
    {
        ActivateParticles(false);
    }

    void ActivateParticles(bool status)
    {
        if(status == true)
        {
            particlePlaceholder1.SetActive(true);
            particlePlaceholder2.SetActive(true);
        }
        else if(status == false)
        {
            particlePlaceholder1.SetActive(false);
            particlePlaceholder2.SetActive(false);
        }
    }
    void SetParticlesPosition()
    {
        //Set Parent
        particleManagerScript.confettiParticles1.transform.SetParent(particlePlaceholder1.transform);
        particleManagerScript.confettiParticles2.transform.SetParent(particlePlaceholder2.transform);

        //Set Position
        particleManagerScript.confettiParticles1.transform.localPosition = new Vector3(0,0,0);

        particleManagerScript.confettiParticles2.transform.localPosition = new Vector3(0, 0, 0);
    }
}
