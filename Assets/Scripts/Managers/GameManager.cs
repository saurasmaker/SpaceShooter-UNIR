using Patterns.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private Vector2 _screenBounds;
    public Vector2 ScreenBounds { get => _screenBounds; }

    public static readonly Dictionary<string, int> DAMAGES_BY_TAG = new Dictionary<string, int>
    {
        { "Projectile", 1 },
        { "Missile", 3 },
        { "Asteroid", 5 },
        { "Alien", 10 },
        { "Player", 3 }
    };

    [SerializeField]
    private PlayerInput _playerInput;
    public PlayerInput ThisPlayerInput
    {
        get
        {
            if (_playerInput == null)
                _playerInput = GetComponent<PlayerInput>();

            _playerInput.enabled = false;
            _playerInput.enabled = true;
            return _playerInput;
        }
    }

    private InputAction _pauseAction = null;
    public InputAction PauseAction
    {
        get
        {
            if (_pauseAction == null)
                _pauseAction = ThisPlayerInput.actions.FindAction("Pause");

            return _pauseAction;
        }
    }

    private InputAction _resumeAction = null;
    public InputAction ResumeAction
    {
        get
        {
            if (_resumeAction == null)
                _resumeAction = ThisPlayerInput.actions.FindAction("Resume");

            return _resumeAction;
        }
    }

    [SerializeField]
    AudioSource _generalSound;
    [SerializeField]
    AudioClip[] _ost;
    private int _ostIndex = 0;
    public int OstIndex
    { 
        get { return _ostIndex; }
        set {
            _ostIndex = value;
            _ostIndex %= _ost.Length;
        }
    }

    [SerializeField]
    private AudioClip _shockDamageSound = null, _missileDamageSound = null, _projectileDamageSound = null, _destroySound = null;
    public AudioClip ShockDamageSound { get { return _shockDamageSound; } }
    public AudioClip MissileDamageSound { get { return _missileDamageSound; } }
    public AudioClip ProjectileDamageSound { get { return _projectileDamageSound; } }
    public AudioClip DestroySound { get { return _destroySound; } }




    [SerializeField]
    private float _maxGameSpeed, _startGameSpeed, _increaseSpeed;

    [SerializeField]
    public UnityEvent OnPauseGame, OnResumeGame, OnResetScene;

    [SerializeField]
    private float _currentGameSpeed;


    public float MaxGameSpeed { get => _maxGameSpeed; }
    public float CurrentGameSpeed { get => _currentGameSpeed; }
    public float StartGameSpeed { get => _startGameSpeed; }

    override protected void OnAwake()
    {
        base.OnAwake();

        Application.targetFrameRate = 60;

        if (_generalSound == null)
            _generalSound = GetComponent<AudioSource>();
        
        _playerInput.enabled = true;
        
        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        
        _currentGameSpeed = _startGameSpeed;
    }

    override protected void OnStart()
    {
        base.OnStart();

        PauseAction.performed += PauseGame;
        ResumeAction.performed += ResumeGame;

        StartIncreaseGameSpeed();
    }

    void Update()
    {
        if (!_generalSound.isPlaying)
        {
            _generalSound.clip = _ost[OstIndex++];
            _generalSound.Play();
        }
    }

    private void OnDestroy()
    {
        PauseAction.performed -= PauseGame;
        ResumeAction.performed -= ResumeGame;
    }

    private void StartIncreaseGameSpeed()
    {
        StartCoroutine(IncreaseGameSpeedRoutine());
    }

    private IEnumerator IncreaseGameSpeedRoutine()
    {
        while(_currentGameSpeed < _maxGameSpeed)
        {
            yield return new WaitForEndOfFrame();
            _currentGameSpeed += Time.deltaTime * _increaseSpeed;

            if(_currentGameSpeed > _maxGameSpeed)
                _currentGameSpeed = _maxGameSpeed;
        }

        yield return null;
    }

    private void PauseGame(InputAction.CallbackContext ctx)
    {
        PauseGame();
    }

    private void ResumeGame(InputAction.CallbackContext ctx)
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        ThisPlayerInput.SwitchCurrentActionMap("UI");
        OnPauseGame?.Invoke();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        ThisPlayerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1.0f;
        OnResumeGame?.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        ThisPlayerInput.SwitchCurrentActionMap("Player");
        OnResetScene?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ExitGame()
    {
        Application.Quit();
    }



    public enum Tags
    {
        Projectile,
        Missile,
        Asteroid,
        Alien,
        Player
    }

}
