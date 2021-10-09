using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;

    private Button _buttonContinue;
    private int _knifeMod;
    private int _gameMod;

    public int KnifeMod => _knifeMod;
    public int GameMod => _gameMod;

    private void OnEnable()
    {
        SetButtonEvent();
    }

    private void OnDisable()
    {
        _buttonContinue.onClick.RemoveListener(ButtonClick);
    }

    public void Activation()
    {
        gameObject.SetActive(true);
    }

    public void Deactivation()
    {
        gameObject.SetActive(false);
    }

    public void SetKnifeMod(int knifeMod)
    {
        _knifeMod = knifeMod;
        SetTextNumKnifeHit();
    }

    public void SetGameMod(int gameMod)
    {
        _gameMod = gameMod;

        switch (_gameMod)
        {
            case 1:
                SetTextNum(1);
                break;
            case 2:
                SetTextNum(2);
                break;
        }
    }

    public void SetButtonEvent()
    {
        _buttonContinue = GetComponentInChildren<Button>();
        _buttonContinue.onClick.AddListener(ButtonClick);
    }

    private void ButtonClick()
    {
        PlayerInput.Instance.OnClick();
        GamemodManager.Instance.OnClick();
    }

    private void SetTextNum(int gameModIndex)
    {
        switch (gameModIndex)
        {
            case 1:
                _textNumber.text = DataManager.Instance.GameData.ProgressData.CurrentStackKnifeLevel.ToString();
                break;
            case 2:
                _textNumber.text = DataManager.Instance.GameData.ProgressData.CurrentKnifeFestLevel.ToString();
                break;
        }
    }

    private void SetTextNumKnifeHit()
    {
        _textNumber.text = (_knifeMod % 3) switch
        {
            0 => (DataManager.Instance.GameData.ProgressData.CurrentMarkLevel + 1).ToString(),
            1 => (DataManager.Instance.GameData.ProgressData.CurrentCubeLevel + 1).ToString(),
            2 => (DataManager.Instance.GameData.ProgressData.CurrentFlatLevel + 1).ToString(),
            _ => throw new System.NotImplementedException(),
        };
    }
}
