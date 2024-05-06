using UnityEngine;
using UnityEngine.Pool;

public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb2d;
    [SerializeField]
    InScreenDetector _screenDetector;
    [SerializeField]
    HealthController _healthController;
    [SerializeField]
    ParticleSystemController _shootParticleSystem, _onImpactParticleSystem;
    [SerializeField]
    GameObject _sprite;
    [SerializeField]
    bool _releaseOnHit = false;
    [SerializeField]
    float _speed;    
    
    private ObjectPool<ProjectileController> _projectilePool;
    public ObjectPool<ProjectileController> ProjectilePool { set { _projectilePool = value; } }


    private void Awake()
    {
        if(_rb2d == null)
            _rb2d = GetComponent<Rigidbody2D>();
        if(_screenDetector == null)
            _screenDetector = GetComponent<InScreenDetector>();

        if (_healthController != null)
        {
            _healthController.OnDie.AddListener(PlayDieParticleSystem);
            _healthController.OnDie.AddListener(HideSprite);
        }


        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();

        _screenDetector.OnExitBounds.AddListener(() => _projectilePool.Release(this));

        if (!_releaseOnHit && _onImpactParticleSystem != null)
            _onImpactParticleSystem.OnStop += Release;
            
    }

    public void SetVelocity(Vector2 velocity) {
        _rb2d.velocity = velocity * (_speed + GameManager.Instance.CurrentGameSpeed);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void Initialize(Vector2 position, Vector3 eulerAngles, Vector2 velocity)
    {
        transform.eulerAngles = eulerAngles;
        SetVelocity(velocity);
        SetPosition(position);
    }

    public void OnEnable()
    {
        if (_shootParticleSystem != null)
            _shootParticleSystem.PlayAnimation();

        if(_sprite != null)
            _sprite.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_releaseOnHit)
            _projectilePool.Release(this);
    }

    private void Release()
    {
        if(_projectilePool != null)
            _projectilePool.Release(this);
    }

    private void PlayDieParticleSystem()
    {
        if (_onImpactParticleSystem != null)
            _onImpactParticleSystem.PlayAnimation();  
    }

    private void HideSprite()
    {
        if (_sprite != null)
            _sprite.SetActive(false);
    }
}
