using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InScreenDetector))]
[RequireComponent(typeof(HealthController))]
public class EnemyController : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    [SerializeField]
    private InScreenDetector _screenDetector;
    [SerializeField]
    private HealthController _healthController;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("Attributes")]
    [SerializeField]
    private int _ratio = 1;
    [SerializeField]
    private float _speed = 1f, _rotationSpeed = 1f;
    [SerializeField]
    private bool _rotating, _randomDir;
    #endregion

    #region Properties
    public InScreenDetector ThisScreenDetector { get { return _screenDetector; } }
    public HealthController ThisHealthController { get { return _healthController; } }
    public SpriteRenderer ThisSpriteRenderer { get { return _spriteRenderer; } }

    public bool RandomDir {  get { return _randomDir; } }
    public bool Rotating { get { return _rotating; } }
    public float Speed { get { return _speed; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public int Ratio { get { return _ratio; } }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if (_screenDetector == null)
            _screenDetector = GetComponent<InScreenDetector>();
        if (_healthController == null)
            _healthController = GetComponent<HealthController>();
    }
}
