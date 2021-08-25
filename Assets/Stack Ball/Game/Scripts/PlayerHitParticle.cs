#pragma warning disable 0649

using UnityEngine;
using Watermelon;

public class PlayerHitParticle : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private LevelController levelController;
    [SerializeField]
    private Transform playerPosition;
    [SerializeField]
    private ProjectData projectData;
    [SerializeField]
    private Watermelon.AudioSettings audioSettings;

    [Space]
    [SerializeField]
    private GameObject hitParticlePrefab;

    [Space]
    [SerializeField]
    private Vector3 particleOffset;
    
    private PoolSimpleComponent<ParticleSystem> hitParticlePool;

    private void Awake()
    {
        hitParticlePool = new PoolSimpleComponent<ParticleSystem>("HitParticle", hitParticlePrefab, 5, true);
    }

    private void OnEnable()
    {
        playerController.onPlayerDestructPlatform += OnPlayerDestructPlatform;
    }

    private void OnDisable()
    {
        playerController.onPlayerDestructPlatform -= OnPlayerDestructPlatform;
    }

    private void OnPlayerDestructPlatform(Platform platform)
    {
        ReturnParticlesToPool();
    }

    public void ReturnParticlesToPool()
    {
        hitParticlePool.ReturnToPoolEverything(true);
    }

    public void OnHit()
    {
        /*ParticleSystem hit = hitParticlePool.GetPooledComponent();
        if(hit != null)
        {
            hit.gameObject.SetActive(true);

            Platform tempPlatform = levelController.NextPlatform;
            if(tempPlatform != null)
            {
                hit.transform.SetParent(tempPlatform.transform);
            }
            else
            {
                hit.transform.SetParent(null);
            }

            hit.transform.position = playerPosition.transform.position + particleOffset;
            hit.Play();
        }*/

        AudioController.PlaySound(audioSettings.sounds.playerHitAudioClip, 0.5f);
    }
}
