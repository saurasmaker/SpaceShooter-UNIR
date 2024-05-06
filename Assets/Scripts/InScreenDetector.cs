using UnityEngine;
using UnityEngine.Events;

public class InScreenDetector : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _sprite;
    public SpriteRenderer ThisSpriteRenderer
    {
        get { return _sprite; }
        set { _sprite = value; }
    }
    [SerializeField]
    public UnityEvent OnExitBounds, OnEnterBounds;
    
    private bool _isOutOfBounds;
    public bool IsOutOfBounds
    {
        get => _isOutOfBounds;

        private set
        {
            if (_isOutOfBounds != value)
            {
                _isOutOfBounds = value;
                if(_isOutOfBounds)
                    OnExitBounds?.Invoke();
                else 
                    OnEnterBounds?.Invoke();
            }
        }
    }

    private void Awake()
    {
        if (_sprite == null)
        {
            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite == null)
                _sprite = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void OnEnable()
    {
        _isOutOfBounds = CheckOutOfBounds();
    }

    private void OnDisable()
    {
        _isOutOfBounds = true;
    }

    private void Update()
    {
        IsOutOfBounds = CheckOutOfBounds();
    }

    private bool CheckOutOfBounds()
    {
        Bounds bounds = _sprite.bounds;
        Vector3 rightTop = Camera.main.WorldToViewportPoint(bounds.max);
        Vector3 leftBottom = Camera.main.WorldToViewportPoint(bounds.min);
        return rightTop.y < 0 || leftBottom.y > 1 || rightTop.x < 0 || leftBottom.x > 1;
    }
}
