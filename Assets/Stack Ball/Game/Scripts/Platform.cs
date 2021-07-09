using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;

public class Platform : MonoBehaviour
{
    [HideInInspector]
    public int platformIndex;

    [SerializeField]
    private Transform[] platformParts;
    private Collider[] platformColliders;
    private Renderer[] platformRenderer;

    private PlatformTweenHolder[] destructPlatformTweenHolder;

    private Vector3[] defaultPositions;
    private Quaternion[] defaultRotations;

    private MaterialPropertyBlock materialPropertyBlock;

    private float pitchValue;

    private bool isInititalised = false;

    private float defaultScale;
    private Vector3 DefaultScaleVector3 => new Vector3(defaultScale, 1, defaultScale);

    private readonly Vector3 SCALE_POSITION = new Vector3(1.4f, 1, 1.4f);

    public void InitializeComponent()
    {
        if (isInititalised)
            return;

        platformColliders = new Collider[platformParts.Length];
        defaultPositions = new Vector3[platformParts.Length];
        defaultRotations = new Quaternion[platformParts.Length];
        platformRenderer = new Renderer[platformParts.Length];
        for (int i = 0; i < platformParts.Length; i++)
        {
            platformColliders[i] = platformParts[i].GetComponent<Collider>();
            platformRenderer[i] = platformParts[i].GetComponent<Renderer>();

            defaultPositions[i] = platformParts[i].transform.localPosition;
            defaultRotations[i] = platformParts[i].transform.localRotation;
        }

        materialPropertyBlock = new MaterialPropertyBlock();

        isInititalised = true;
    }

    public void ScalePlatform()
    {
        transform.DOScale(SCALE_POSITION * defaultScale, 0.15f, false, TweenType.FixedUpdate).SetEasing(Ease.Type.BackIn).OnComplete(delegate
        {
            transform.DOScale(DefaultScaleVector3, 0.15f, false, TweenType.FixedUpdate);
        });
    }

    public void Init(int platformIndex, byte[] elements, Color platformColor, float pitchValue, float scale, Material defaultMaterial, Material obstacleMaterial)
    {
        InitializeComponent();

        this.platformIndex = platformIndex;
        this.pitchValue = pitchValue;
        this.defaultScale = scale;

        if (!destructPlatformTweenHolder.IsNullOrEmpty())
        {
            for(int i = 0; i < destructPlatformTweenHolder.Length; i++)
            {
                destructPlatformTweenHolder[i].Kill();
            }

            destructPlatformTweenHolder = null;
        }

        ResetPositions();

        for (int i = 0; i < elements.Length; i++)
        {
            switch(elements[i])
            {
                case 0:
                    platformRenderer[i].material = defaultMaterial;

                    platformRenderer[i].GetPropertyBlock(materialPropertyBlock);
                    materialPropertyBlock.SetColor("_Tint", platformColor);
                    platformRenderer[i].SetPropertyBlock(materialPropertyBlock);

                    platformParts[i].gameObject.tag = "Untagged";

                    break;
                case 1:
                    if(platformRenderer[i].HasPropertyBlock())
                    {
                        platformRenderer[i].SetPropertyBlock(null);
                    }

                    platformRenderer[i].material = obstacleMaterial;

                    platformParts[i].gameObject.tag = "Obstacle";
                    break;
            }
        }
    }

    public void ResetPositions()
    {
        for (int i = 0; i < platformColliders.Length; i++)
        {
            platformParts[i].transform.localPosition = defaultPositions[i];
            platformParts[i].transform.localRotation = defaultRotations[i];

            platformColliders[i].enabled = true;
        }

        transform.localScale = DefaultScaleVector3;
    }
    
    public void DestructObject()
    {
        PlayerController.PlayDestructSound(pitchValue);

        for (int i = 0; i < platformColliders.Length; i++)
        {
            platformColliders[i].enabled = false;
        }

        destructPlatformTweenHolder = new PlatformTweenHolder[platformParts.Length];
        for (int i = 0; i < platformParts.Length; i++)
        {
            int index = i;
            Vector3 movePosition;

            if(transform.position.x - platformParts[i].transform.position.x >= 0) // Throw left
            {
                movePosition = transform.position + new Vector3(-10, 10, 0);
            }
            else
            {
                movePosition = transform.position + new Vector3(10, 10, 0);
            }

            destructPlatformTweenHolder[i] = new PlatformTweenHolder();
            destructPlatformTweenHolder[i].xPositionTweenCase = platformParts[i].DOMoveX(movePosition.x + Random.Range(-2, 2), 1, false, TweenType.FixedUpdate).OnComplete(() => gameObject.SetActive(false));
            destructPlatformTweenHolder[i].yPositionTweenCase = platformParts[i].DOMoveY(movePosition.y, 1, false, TweenType.FixedUpdate);
            destructPlatformTweenHolder[i].rotationPositionTweenCase = platformParts[i].DOLocalRotate(Quaternion.Euler(platformParts[i].eulerAngles + new Vector3(Random.Range(-40, 40), Random.Range(-40, 40), Random.Range(-40, 40))), 0.4f, false, TweenType.FixedUpdate);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Pick Transforms")]
    [Button("Pick Transforms")]
    public void PickTransforms()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        platformParts = new Transform[colliders.Length];
        for(int i = 0; i < platformParts.Length; i++)
        {
            platformParts[i] = colliders[i].GetComponent<Transform>();
        }
    }
#endif

    private class PlatformTweenHolder
    {
        public TweenCase xPositionTweenCase = null;
        public TweenCase yPositionTweenCase = null;
        public TweenCase rotationPositionTweenCase = null;

        public void Kill()
        {
            if (xPositionTweenCase != null && !xPositionTweenCase.isCompleted)
                xPositionTweenCase.Kill();

            if (yPositionTweenCase != null && !yPositionTweenCase.isCompleted)
                yPositionTweenCase.Kill();

            if (rotationPositionTweenCase != null && !rotationPositionTweenCase.isCompleted)
                rotationPositionTweenCase.Kill();
        }
    }
}
