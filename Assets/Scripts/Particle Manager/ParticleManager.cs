using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] public GameObject transformParticle;
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
        if (state == GameManager.GameState.Transform) { }
            //Play Particles
    }

    public IEnumerator PlayTransformParticle(Transform vehicle)
    {
        transformParticle.transform.SetParent(vehicle);
        transformParticle.transform.localPosition = new Vector3(0, 0, 0);
        float time = 0;
        bool runOnce = true;
        while (time < 1)
        {
            if (runOnce)
            {
                transformParticle.SetActive(true);
                runOnce = false;
            }
            yield return null;
        }
        transformParticle.SetActive(false);
    }
}
