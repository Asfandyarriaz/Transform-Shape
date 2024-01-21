using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] public GameObject transformParticlePlayer;
    [SerializeField] public GameObject transformParticleAI;
    [SerializeField] public GameObject transformParticleAI2;
    [SerializeField] public GameObject transformParticleAI3;
    [SerializeField] private float transformParticleDuration;

    [Header("Win Particles")]
    [SerializeField] public GameObject confettiParticles1;
    [SerializeField] public GameObject confettiParticles2;

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

        if (state == GameManager.GameState.Start)
        {

        }
    }
    public IEnumerator PlayTransformParticle(Transform vehicle, GameObject particle)
    {
        particle.transform.SetParent(vehicle);
        particle.transform.localPosition = new Vector3(0, 0, 0);
        particle.transform.localRotation = Quaternion.identity;
        float time = 0;
        bool runOnce = true;
        while (time < transformParticleDuration)
        {
            if (runOnce)
            {
                particle.SetActive(true);
                runOnce = false;
            }
            yield return null;
        }
        particle.SetActive(false);
    }
}
