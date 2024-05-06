using Patterns.Singletons;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    [Header("References")]

    [SerializeField]
    private PolygonCollider2D _polygonCollider;
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private ShooterController _mainShooterController;
    [SerializeField]
    ParticleSystemController _dieParticleSystem;
    [SerializeField]
    HealthController _healthController;
    [SerializeField]
    AudioSource _audioSource;
    [SerializeField]
    AudioClip _shockSound;

    [Header("Attributes")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _hp, _currentHp;


    private Vector2 _playerArea;
    private PlayerInput _playerInput;


    public HealthController ThisHealthController { get => _healthController; }


    #region Actions
    private InputAction _moveAction = null;
    public InputAction MoveAction
    {
        get
        {
            if (_moveAction == null)
                _moveAction = _playerInput.actions.FindAction("Move");

            return _moveAction;
        }

    }

    private InputAction _fireAction = null;
    public InputAction FireAction
    {
        get
        {
            if (_fireAction == null)
                _fireAction = _playerInput.actions.FindAction("Fire");

            return _fireAction;
        }
    }

    
    #endregion


    #region MonoBehaviour Methods
    protected override void OnAwake()
    {
        base.OnAwake();

        SetComponents();
    }

    protected override void OnStart()
    {
        base.OnStart();

        SetInputs();

        SetEvents();

        GameManager.Instance.OnResetScene.AddListener(UnsetInputs);
        _playerArea = new Vector2(_spriteRenderer.bounds.size.x, _spriteRenderer.bounds.size.y) / 2;
    }

    private void Update()
    {
        StayInScreen();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Asteroid" || tag == "Alien")
            ShockSound();
    }

    private void OnDestroy()
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }
    }
    #endregion




    private void StayInScreen()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, -1 * GameManager.Instance.ScreenBounds.x + _playerArea.x, GameManager.Instance.ScreenBounds.x - _playerArea.x);
        viewPos.y = Mathf.Clamp(viewPos.y, -1 * GameManager.Instance.ScreenBounds.y + _playerArea.y, GameManager.Instance.ScreenBounds.y - _playerArea.y);
        transform.position = viewPos;
    }



    #region Initialize Methods
    /// <summary>
    /// 
    /// </summary>
    private void SetComponents()
    {
        if (_mainShooterController == null)
            Debug.LogWarning("No main shooter controller referenced...");

        if(_rigidbody2D == null)
            _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_healthController == null)
            _healthController = GetComponentInChildren<HealthController>();

        if (_polygonCollider == null)
            _polygonCollider = GetComponent<PolygonCollider2D>();

        if(_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetInputs()
    {
        _playerInput = GameManager.Instance.ThisPlayerInput;

        MoveAction.performed += Move;
        FireAction.performed += Fire;
        MoveAction.canceled += StopMove; 
    }

    private void UnsetInputs()
    {
        MoveAction.performed -= Move;
        FireAction.performed -= Fire;
        MoveAction.canceled -= StopMove;

        GameManager.Instance.OnResetScene.RemoveListener(UnsetInputs);
    }


    private void SetEvents()
    {
        _healthController.OnDie.AddListener(Die);
    }

    #endregion

    private Coroutine _moveCoroutine = null;
    /// <summary>
    /// 
    /// </summary>
    private void Move(InputAction.CallbackContext ctx)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        _moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            _rigidbody2D.velocity = MoveAction.ReadValue<Vector2>() * _speed;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void StopMove(InputAction.CallbackContext ctx)
    {
        StopCoroutine(_moveCoroutine);
        _moveCoroutine = null;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Fire(InputAction.CallbackContext ctx)
    {
        _mainShooterController.Shoot();
    }


    /// <summary>
    /// 
    /// </summary>
    private void Die()
    {
        _playerInput.enabled = false;
        _polygonCollider.enabled = false;
        _spriteRenderer.gameObject.SetActive(false);
        if (_dieParticleSystem != null)
        {
            _dieParticleSystem.OnStop += PauseMenuController.Instance.PlayerDie;
            _dieParticleSystem.PlayAnimation();
        }
        else PauseMenuController.Instance.PlayerDie();
    }


    private void ShockSound()
    {
        if (_audioSource == null)
            return;

        _audioSource.clip = _shockSound;
        if (_audioSource.clip == null)
            return;

        _audioSource.Play();
    }
 
}
