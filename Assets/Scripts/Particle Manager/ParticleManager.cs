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

/*  [Header("Player Marker")]
    [SerializeField] GameObject playerMarker;
    [SerializeField] TMP_Text playerName;*/
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

        if (state == GameManager.GameState.Start)
        {
            /*if (PlayerDataController.Instance.playerData.playerName != null)
            {
                playerName.text = PlayerDataController.Instance.playerData.playerName;
            }
            else
            {
                playerName.text = "YOU";
            }*/
        }

    }

    public IEnumerator PlayTransformParticle(Transform vehicle, GameObject particle)
    {
        particle.transform.SetParent(vehicle);
        particle.transform.localPosition = new Vector3(0, 0, 0);
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

    /*public void FollowPlayerMarker(Transform Parent)
    {
        playerMarker.transform.SetParent(Parent);
        playerMarker.transform.localPosition = new Vector3(0, 2, 0.042f);
        playerMarker.transform.rotation = Quaternion.Euler(0, -30.747f, 0);
    }*/
}
