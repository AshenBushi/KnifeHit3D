using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlowMode : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _timer;

    private IEnumerator SlowGame()
    {
        Time.timeScale = 0.5f;
        _button.interactable = false;
        var timer = 5f;
        _text.text = "";
        
        while (timer > 0)
        {
            timer -= Time.deltaTime * 2;
            _timer.text = timer.ToString("#.##");
            yield return null;
        }

        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    public void ActiveSlowMode()
    {
        StartCoroutine(SlowGame());
        DataManager.GameData.PlayerData.SlowMode--;
        DataManager.Save();
    }
}
