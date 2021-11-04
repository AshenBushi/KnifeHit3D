using System.Collections;
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
        TargetHandler.Instance.EnableSlowMode();
        
        _button.interactable = false;
        
        var timer = 5f;
        _text.text = "";
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _timer.text = timer.ToString("#.##");
            yield return null;
        }
        
        TargetHandler.Instance.DisableSlowMode();
        
        gameObject.SetActive(false);
    }

    public void ActiveSlowMode()
    {
        StartCoroutine(SlowGame());
        DataManager.Instance.GameData.PlayerData.SlowMode--;
        DataManager.Instance.Save();
    }
}
