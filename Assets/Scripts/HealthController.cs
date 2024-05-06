using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float _maxHp, _currentHp;
    [SerializeField]
    private Gradient _hpBarColor;

    [Header("UI References")]
    [SerializeField]
    TMP_Text _hpText;
    [SerializeField]
    Slider _hpSlider;
    [SerializeField]
    Image _sliderFill;

    [Header("Events")]
    [SerializeField]
    public UnityEvent OnDie;
    public UnityEvent OnTakeDamage;


    

    private void OnEnable()
    {
        Initialize();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        ReceiveDamage(tag);
    }

    private void Initialize()
    {
        _currentHp = _maxHp;
        
        if (_hpText != null)
            _hpText.text = _currentHp + " / " + _maxHp;
        
        if(_hpSlider != null)
        {
            _hpSlider.maxValue = _maxHp;
            _hpSlider.minValue = 0;
            _hpSlider.value = _currentHp;
        } 
    }

    public void ReceiveDamage(string tag)
    {
        _currentHp -= GameManager.DAMAGES_BY_TAG[tag];
        if (_hpText != null)
            _hpText.text = _currentHp + " / " + _maxHp;
        if (_hpSlider != null)
            _hpSlider.value = _currentHp;
        if (_sliderFill != null)
            _sliderFill.color = _hpBarColor.Evaluate(_currentHp / _maxHp);

        OnTakeDamage?.Invoke();

        if (_currentHp <= 0)
            OnDie?.Invoke();

    }
}
