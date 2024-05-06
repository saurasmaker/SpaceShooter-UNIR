using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShooterController : MonoBehaviour
{
    [SerializeField]
    Transform[] _guns;
    [SerializeField]
    ProjectilePool _projectilePool;
    [SerializeField]
    ShootingPattern _pattern;
    [SerializeField]
    bool _allGunsAtOnce = false, _autoShoot;
    [SerializeField]
    Vector2 _direction = Vector2.zero;
    [SerializeField]
    Vector3 _angle = Vector3.zero;
    [SerializeField]
    float _cadenceSecs = 1f;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystemController _particleSystemController;

    private int _gunIndex = 0;
    public int GunIndex
    {
        get { return _gunIndex; }
        set
        {
            _gunIndex = value;
            _gunIndex %= _guns.Length;
        }
    }

    public ProjectilePool ThisProjectilePool { get { return _projectilePool; } }


    public void OnEnable()
    {
        if(_autoShoot)
            StartCoroutine(ShootRoutine());
    }


    public void Shoot()
    {
        if(_particleSystemController != null)
            _particleSystemController.PlayAnimation();

        if (_allGunsAtOnce)
            for (int i = 0; i < _guns.Length; ++i)
                _projectilePool.ProjectilesPool.Get().Initialize(_guns[i].position, _angle, _direction);
            
        else
            _projectilePool.ProjectilesPool.Get().Initialize(_guns[GunIndex++].position, _angle, _direction);
    }

    public ObjectPool<ProjectileController> GetPool()
    {
        return _projectilePool.ProjectilesPool;
    }


    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_cadenceSecs);
            Shoot();
        }
    }


    enum ShootingPattern
    {
        Straight,
        Circle, 
        Spiral
    }
}
