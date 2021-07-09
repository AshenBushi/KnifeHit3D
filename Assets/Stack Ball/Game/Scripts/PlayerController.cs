#pragma warning disable 0649

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Watermelon;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private GameObject playerDestructedModel;
    [SerializeField]
    private ParticleSystem playerTrailParticleSystem;
    [SerializeField]
    private ProjectData projectData;
    [SerializeField]
    private Watermelon.AudioSettings audioSettings;

    [Space]
    [SerializeField]
    private float playerOffset = 0.45f;

    [Space]
    [SerializeField]
    private LevelController levelController;
    
    [LineSpacer("Combo")]
    [SerializeField]
    private GameObject comboObject;
    [SerializeField]
    private Image comboFillbarImage;
    [SerializeField]
    private GameObject comboWarningObject;
    [SerializeField]
    private ParticleSystem comboParticleSystem;
    
    private int comboStartColums = 5;
    private int comboRequireColums = 10;
    private float comboActiveTime = 1.0f;

    private float comboTimePerDestruct;

    private bool isComboEnabled;
    private float comboTimer;
    private bool isComboReached;

    private int destructInARow = 0;

    private int downParameterHash;

    private Coroutine jumpCoroutine;
    private TweenCase movementTween;

    private Rigidbody[] destructRigidbodies;
    private Vector3[] desctructPlayerPositions;

    public OnPlayerDown onPlayerDown;
    public OnPlayerDestructPlatform onPlayerDestructPlatform;
    public OnGameStarted onGameStarted;

    private WaitForFixedUpdate fallWait;

    private bool isEnabled = true;
    private bool isGameStarted = false;
    public bool IsGameStarted
    {
        get { return isGameStarted; }
    }

    private TweenCase disableTweenCase;

    private bool mouseDown = false;

    private float vibrationCooldown = 0.07f;
    private float vibrationTime = float.MinValue;

    private const float PITCH_MIN = 1.0f;
    private const float PITCH_MAX = 2.0f;
    
    private void Awake()
    {
        instance = this;

        downParameterHash = Animator.StringToHash("Down");
        
        destructRigidbodies = playerDestructedModel.GetComponentsInChildren<Rigidbody>();
        desctructPlayerPositions = new Vector3[destructRigidbodies.Length];
        for(int i = 0; i < desctructPlayerPositions.Length; i++)
        {
            desctructPlayerPositions[i] = destructRigidbodies[i].transform.localPosition;
        }
        
        comboStartColums = projectData.comboStartColums;
        comboRequireColums = projectData.comboRequireColums;
        comboActiveTime = projectData.comboActiveTime;

        comboTimePerDestruct = (float)1 / comboRequireColums;

        fallWait = new WaitForFixedUpdate();
    }

    public void Init()
    {
        Platform platform = levelController.NextPlatform;
        if(platform != null)
            playerTransform.position = playerTransform.position.SetY(platform.transform.position.y + playerOffset);

        // Disable combo effect
        DisableCombo();

        // Reset player model
        playerDestructedModel.SetActive(false);
        playerTransform.gameObject.SetActive(true);

        playerAnimator.Play("Idle", -1, 0);

        isEnabled = true;
        isGameStarted = false;
    }

    private void DisableCombo()
    {
        playerTrailParticleSystem.Play();
        comboParticleSystem.Stop();

        isComboReached = false;
        isComboEnabled = false;

        comboObject.SetActive(false);
        comboWarningObject.SetActive(false);

        destructInARow = 0;
        comboTimer = 0;
    }

    private void Update()
    {
        if (!isEnabled)
            return;
        
        if(isComboEnabled)
        {
            if(!mouseDown)
            {
                comboTimer -= Time.deltaTime / comboActiveTime;
            }
            else if(isComboReached)
            {
                comboTimer -= Time.deltaTime / comboActiveTime;

                if(comboTimer <= 0.2f)
                {
                    comboWarningObject.SetActive(true);
                }
            }

            comboFillbarImage.fillAmount = comboTimer;

            if (comboTimer <= 0)
            {
                DisableCombo();
            }
        }
    }

    public void MouseDown()
    {
        if (mouseDown)
            return;

        mouseDown = true;

        if (!isGameStarted)
        {
            onGameStarted.Invoke();

            isGameStarted = true;
        }

        playerAnimator.SetBool(downParameterHash, true);

        jumpCoroutine = StartCoroutine(JumpCoroutine());

        if (onPlayerDown != null)
            onPlayerDown.Invoke(true);
    }

    public void MouseUp()
    {
        if (isEnabled)
        {
            if (destructInARow > 1)
            {
                Platform tempPlatform = levelController.NextPlatform;
                if (tempPlatform != null)
                {
                    bool isObstacleHitted = false;
                    Collider[] hitColliders = Physics.OverlapSphere(playerTransform.transform.position.SetY(tempPlatform.transform.position.y), 0.1f);
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if (hitColliders[i].CompareTag("Obstacle"))
                        {
                            isObstacleHitted = true;

                            break;
                        }
                    }

                    if (isObstacleHitted)
                    {
                        LongVibration();

                        tempPlatform.ScalePlatform();

                        PlayBounceSound();
                    }
                }
            }
        }

        playerAnimator.SetBool(downParameterHash, false);

        if (onPlayerDown != null)
            onPlayerDown.Invoke(false);

        mouseDown = false;
    }

    private bool CheckPlatformGround(Platform platform)
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.transform.position.SetY(platform.transform.position.y), 0.1f);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Obstacle"))
            {
                return true;
            }
        }

        return false;
    }
    
    public void Revive()
    {
        for (int i = 0; i < destructRigidbodies.Length; i++)
        {
            destructRigidbodies[i].gameObject.SetActive(false);
        }
        
        playerDestructedModel.SetActive(false);
        playerTransform.gameObject.SetActive(true);

        DisableCombo();

        isEnabled = true;

        GameController.Revive();
    }

    private void DestructPlayer()
    {
        if (disableTweenCase != null && !disableTweenCase.isCompleted) 
            disableTweenCase.Kill();

        if (destructRigidbodies != null)
        {
            for (int i = 0; i < destructRigidbodies.Length; i++)
            {
                if(destructRigidbodies[i] != null)
                {
                    destructRigidbodies[i].velocity = Vector3.zero;
                    destructRigidbodies[i].transform.localPosition = desctructPlayerPositions[i];
                    destructRigidbodies[i].gameObject.SetActive(true);
                }
            }
        }

        playerTransform.gameObject.SetActive(false);

        playerDestructedModel.transform.position = playerTransform.position;
        playerDestructedModel.SetActive(true);

        if (destructRigidbodies != null)
        {
            for (int i = 0; i < destructRigidbodies.Length; i++)
            {
                destructRigidbodies[i].AddExplosionForce(5f, playerDestructedModel.transform.position.SetZ(0), 10, 0, ForceMode.Impulse);
            }

            disableTweenCase = Tween.DelayedCall(2, delegate
            {
                for (int i = 0; i < destructRigidbodies.Length; i++)
                {
                    destructRigidbodies[i].gameObject.SetActive(false);
                }
            });
        }
    }
    
    private IEnumerator JumpCoroutine()
    {
        destructInARow = 0;

        bool jump = true;
        while(jump)
        {
            if (movementTween != null)
            {
                while (!movementTween.isCompleted)
                {
                    yield return fallWait;
                }
            }
            
            if (!mouseDown)
                yield break;

            Platform tempPlatform = levelController.NextPlatform;
            if (tempPlatform != null)
            {
                bool isObstacleHitted = CheckPlatformGround(tempPlatform);

                if (isObstacleHitted && !isComboReached)
                {
                    if(destructInARow > 0)
                    {
                        float timer = projectData.playerSafeTime;
                        while (timer > 0)
                        {
                            timer -= Time.deltaTime;

                            if (!mouseDown)
                                yield break;

                            yield return fallWait;
                        }
                    }
                    
                    tempPlatform.ScalePlatform();

                    DestructPlayer();

                    AudioController.PlaySound(audioSettings.sounds.gameOverAudioClip, 1);

                    isEnabled = false;

                    comboObject.SetActive(false);

                    MouseUp();

                    GameController.GameOver();

                    yield break;
                }
                else
                {
                    movementTween = playerTransform.DOMoveY(tempPlatform.transform.position.y - 0.5f + playerOffset, projectData.playerFallTime, false, TweenType.FixedUpdate).OnComplete(delegate
                    {
                        if(audioSettings.isVibrationEnabled)
                        {
                            if (GameSettingsPrefs.Get<bool>("vibration"))
                            {
                                if (audioSettings.vibrations.shortVibration != 0)
                                {
                                    if (instance.vibrationTime <= Time.time)
                                    {
                                        Vibration.Vibrate(audioSettings.vibrations.shortVibration);

                                        instance.vibrationTime = Time.time + instance.vibrationCooldown;
                                    }
                                }
                            }
                        }

                        GameController.Score++;

                        destructInARow++;

                        levelController.DestructPlatform(tempPlatform);

                        onPlayerDestructPlatform.Invoke(tempPlatform);

                        if (levelController.PlatformsQueueCount == 0)
                        {
                            playerTrailParticleSystem.Play();
                            comboParticleSystem.Stop();

                            comboObject.SetActive(false);

                            GameController.WinGame();

                            isEnabled = false;

                            MouseUp();

                            jump = false;
                        }
                        else
                        {
                            if(!isComboReached && destructInARow > comboStartColums)
                            {
                                if(!isComboEnabled)
                                {
                                    comboObject.SetActive(true);
                                    comboFillbarImage.color = Color.white;

                                    isComboEnabled = true;
                                }

                                comboTimer += comboTimePerDestruct;

                                if(comboTimer >= 1)
                                {
                                    playerTrailParticleSystem.Stop();
                                    comboParticleSystem.Play();

                                    isComboReached = true;
                                    comboFillbarImage.color = Color.red;
                                    comboFillbarImage.fillAmount = 1;
                                }
                            }

                            if(destructInARow > 1 && !mouseDown)
                            {
                                tempPlatform = levelController.NextPlatform;

                                isObstacleHitted = CheckPlatformGround(tempPlatform);

                                if (isObstacleHitted)
                                {
                                    LongVibration();

                                    tempPlatform.ScalePlatform();

                                    PlayBounceSound();
                                }
                            }
                        }
                    });
                }
            }
        }
    }

    private void LongVibration()
    {
        if (GameSettingsPrefs.Get<bool>("vibration"))
        {
            if(audioSettings.vibrations.longVibration != 0)
            {
                Vibration.Vibrate(audioSettings.vibrations.longVibration);
            }
        }
    }

    public static void PlayDestructSound(float pitch)
    {
        AudioController.PlaySound(instance.audioSettings.sounds.platformDestructAudioClip, 1, Mathf.Lerp(PITCH_MAX, PITCH_MIN, pitch));
    }

    public static void PlayBounceSound()
    {
        AudioController.PlaySound(instance.audioSettings.sounds.platformBounceAudioClip, 1);
    }

    public delegate void OnPlayerDown(bool state);
    public delegate void OnPlayerDestructPlatform(Platform platform);
    public delegate void OnGameStarted();
}
