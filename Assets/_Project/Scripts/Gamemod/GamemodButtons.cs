using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GamemodButtons : MonoBehaviour
{
    private const float DefaultDuration = 0.3f;
    
    [SerializeField] private List<Button> _buttons;
    
    [Header("Sizes")]
    [SerializeField] private Vector3 _selected;
    [SerializeField] private Vector3 _unselected;
    
    private Tween _tween;
    
    public void ChangeButtonSize(GamemodName gamemod, int cameraViewIndex, float duration = DefaultDuration)
    {
        if (gamemod == GamemodName.Lottery)
        {
            foreach (var button in _buttons)
            {
                _tween = button.transform.DOScale(_unselected, duration);
            }

            return;
        }
        
        for (var i = 0; i < _buttons.Count; i++)
        {
            _tween = _buttons[i].transform.DOScale(i == (int) gamemod + 3 * cameraViewIndex ? _selected : _unselected, duration);
        }
    }
}
