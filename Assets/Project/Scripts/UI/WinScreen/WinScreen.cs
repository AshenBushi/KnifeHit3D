using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinScreen : UIScreen
{
    [SerializeField] private GameObject _cup;
    
    private bool _isALottery = false;

    public event UnityAction IsCanStartLottery;

    private IEnumerator WinAnimation()
    {
        yield return new WaitForSeconds(1f);
        
        Enable();
    }

    public override void Enable()
    {
        base.Enable();
        _cup.SetActive(true);
        SoundManager.PlaySound(SoundNames.Win);
    }

    public override void Disable()
    {
        base.Enable();
        _cup.SetActive(false);
    }

    public void Win(bool isALottery)
    {
        StartCoroutine(WinAnimation());
        _isALottery = isALottery;
    }

    public void Continue()
    {
        SoundManager.PlaySound(SoundNames.ButtonClick);
        
        if (_isALottery)
        {
            Disable();
            IsCanStartLottery?.Invoke();
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }
}
