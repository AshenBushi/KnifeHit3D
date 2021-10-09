﻿#pragma warning disable 0414, 0649

using UnityEngine;

namespace Watermelon
{
    public class SettingsPanel : MonoBehaviour
    {
        private static SettingsPanel instance;

        [DrawReference]
        [SerializeField] SettingsAnimation settingsAnimation;

        [Header("Panel Paddings")]
        [SerializeField] float xPanelPosition;
        [SerializeField] float yPanelPosition;

        [Header("Element Paddings")]
        [SerializeField] float elementSpace;

        [SerializeField] SettingsButtonInfo[] settingsButtonsInfo;
        public SettingsButtonInfo[] SettingsButtonsInfo 
        {
            get { return settingsButtonsInfo; }
        }

        private bool isActive = false;
        public bool IsActive => isActive;

        private bool isAnimationActive = false;

        private Vector2[] buttonPositions;
        public Vector2[] ButtonPositions
        {
            get { return buttonPositions; }
        }

        private void Awake()
        {
            instance = this;

            // Disable all buttons
            //for (int i = 0; i < settingsButtonsInfo.Length; i++)
            //{
            //    settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(false);
            //}

            InitAnimation();
            InitPositions();
        }

        public void InitAnimation()
        {
            settingsAnimation.Init(this);
        }

        public void InitPositions()
        {
            Vector2 lastPosition = new Vector2(xPanelPosition, yPanelPosition);

            buttonPositions = new Vector2[settingsButtonsInfo.Length];
            for (int i = 0; i < buttonPositions.Length; i++)
            {
                if(settingsButtonsInfo[i].SettingsButton != null)
                {
                    settingsButtonsInfo[i].SettingsButton.Init(i, this);

                    if (settingsButtonsInfo[i].SettingsButton.IsActive())
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            settingsButtonsInfo[i].SettingsButton.RectTransform.gameObject.SetActive(true);
#endif

                        RectTransform button = settingsButtonsInfo[i].SettingsButton.RectTransform;

                        Vector2 buttonPosition = lastPosition;

                        lastPosition -= new Vector2(0, elementSpace);

                        button.anchoredPosition = new Vector2(xPanelPosition, buttonPosition.y);

                        buttonPositions[i] = buttonPosition;
                    }
                    else
                    {
#if UNITY_EDITOR
                        if (!Application.isPlaying)
                            settingsButtonsInfo[i].SettingsButton.RectTransform.gameObject.SetActive(false);
#endif

                        buttonPositions[i] = Vector3.zero;
                    }
                }
                else
                {
                    Debug.Log("[Settings Panel]: Button reference is missing!");
                }
            }
        }

        public void SettingsButton()
        {
            if (isAnimationActive) return;

            SoundManager.Instance.PlaySound(SoundName.ButtonClick);
            
            if (isActive)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            isAnimationActive = true;

            StartCoroutine(settingsAnimation.Show(delegate
            {
                isAnimationActive = false;
            }));
            
            isActive = true;
        }

        public void Hide(bool immediately = false)
        {
            if(!immediately)
            {
                isAnimationActive = true;

                StartCoroutine(settingsAnimation.Hide(delegate
                {
                    isAnimationActive = false;
                }));
            }
            else
            {
                //for (int i = settingsButtonsInfo.Length - 1; i >= 0; i--)
                //{
                //    settingsButtonsInfo[i].SettingsButton.gameObject.SetActive(false);
                //}
            }
            
            isActive = false;
        }

        public void OnValidate()
        {
            InitPositions();
        }

        public static void HidePanel(bool immediately = false)
        {
            if(instance != null)
                instance.Hide(immediately);
        }

        public static bool IsPanelActive()
        {
            if(instance != null)
                return instance.isActive;

            return false;
        }

        [System.Serializable]
        public class SettingsButtonInfo
        {
            public SettingsButtonBase SettingsButton { get => settingsButton; set => settingsButton = value; }
            [SerializeField] SettingsButtonBase settingsButton;
        }
    }
}

// -----------------
// Settings Panel v 0.2
// -----------------

// Changelog
// v 0.2
// • Fast toggle fix
// v 0.1
// • Added basic version
// • Added example prefab