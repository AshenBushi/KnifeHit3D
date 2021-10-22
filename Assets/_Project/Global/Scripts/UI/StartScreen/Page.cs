using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Page : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumber;
    [SerializeField] private Animator _animator;

    private Button _buttonContinue;
    private CanvasGroup _canvasGroup;
    private int _knifeMod;
    private int _gameMod;
    private int _currentMovie;

    public int KnifeMod => _knifeMod;
    public int GameMod => _gameMod;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _animator.playbackTime = 0f;
    }

    private void OnEnable()
    {
        _buttonContinue = GetComponentInChildren<Button>();
        _buttonContinue.onClick.AddListener(ButtonClick);
    }

    private void OnDisable()
    {
        _buttonContinue.onClick.RemoveListener(ButtonClick);
    }

    public void InitMovie(int currentMovie)
    {
        _currentMovie = currentMovie;
        _animator.SetBool(_currentMovie.ToString(), true);
        _animator.playbackTime = 0f;
    }

    public void Activation()
    {
        _animator.playbackTime = 0f;
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Deactivation()
    {
        _animator.playbackTime = 0f;
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
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
