using System;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController[] _possibleEnemies;

    [Header("References")]
    [SerializeField]
    private Rigidbody2D _rigidbody2D;
    [SerializeField]
    private ParticleSystemController _particleSystemController;
    [SerializeField]
    private Transform _canvas;

    private ObjectPool<EnemyManager> _asteroidsPool;
    public ObjectPool<EnemyManager> AsteroidsPool { set =>  _asteroidsPool = value; }

    private int _randomMax = 0;
    private Vector3 _canvasOffset = Vector3.zero;


    private EnemyController _currentEnemy;

    private void Awake()
    {
        if (_rigidbody2D == null)
            _rigidbody2D = GetComponentInChildren<Rigidbody2D>();

        if (_particleSystemController == null)
            _particleSystemController = GetComponentInChildren<ParticleSystemController>();

        _particleSystemController.OnStop += () => {
            gameObject.SetActive(false);
            _asteroidsPool.Release(this);
        };

        foreach (EnemyController enemy in _possibleEnemies)
            _randomMax += enemy.Ratio;
    }

    private void Update()
    {
        _canvas.position = _rigidbody2D.transform.position + _canvasOffset;
        _particleSystemController.transform.position = _rigidbody2D.transform.position;
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        if (_currentEnemy == null)
            return; 

        _currentEnemy.ThisHealthController.OnTakeDamage.RemoveAllListeners();
        _currentEnemy.ThisHealthController.OnDie.RemoveAllListeners();
        _currentEnemy.ThisScreenDetector.OnExitBounds.RemoveAllListeners();
        _currentEnemy.gameObject.SetActive(false);
    }

    private void Initialize()
    {
        _currentEnemy = GetPossibleEnemy();
        if(_currentEnemy == null)
        {
            Debug.LogWarning("Error getting Possible Enemy!");
            return;
        }

        _currentEnemy.gameObject.SetActive(true);

        _currentEnemy.ThisScreenDetector.OnExitBounds.AddListener(Release);

        _currentEnemy.ThisHealthController.OnTakeDamage.AddListener(TakeDamage);
        _currentEnemy.ThisHealthController.OnDie.AddListener(Destroy);

        _canvas.gameObject.SetActive(false);
        _canvas.transform.localPosition = Vector3.zero;
        _canvasOffset.y = _currentEnemy.ThisSpriteRenderer.size.y;

        _rigidbody2D.gameObject.SetActive(true);
        _rigidbody2D.transform.localPosition = Vector3.zero;
        _rigidbody2D.velocity = (GameManager.Instance.CurrentGameSpeed + _currentEnemy.Speed) 
            * (_currentEnemy.RandomDir ? (Vector2.left + Vector2.up * UnityEngine.Random.Range(-0.5f, 0.5f)) : Vector2.left);
        _rigidbody2D.angularVelocity = _currentEnemy.Rotating ? _currentEnemy.RotationSpeed : 0;
    }

    public void TakeDamage()
    {
        _canvas.gameObject.SetActive(true);
    }

    public void Destroy()
    {
        _rigidbody2D.gameObject.SetActive(false);
        _canvas.gameObject.SetActive(false);

        _particleSystemController.PlayAnimation();
    }

    public void Release()
    {
        _asteroidsPool.Release(this);
    }

    private EnemyController GetPossibleEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, _randomMax+1);
        int currentRatio = 0;
        EnemyController randomEnemy = null;
        foreach (EnemyController pe in _possibleEnemies)
        {
            currentRatio += pe.Ratio;
            if (randomIndex <= currentRatio)
            {
                randomEnemy = pe;
                break;
            }
        }

        return randomEnemy;
    }
}