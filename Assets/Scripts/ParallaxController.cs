using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField]
    GameObject _lastBackground;
    [SerializeField]
    float _speed = 1.0f;

    Vector2 _initPos;
    Vector2 _lastPos;


    void Awake()
    {
        _initPos = transform.position;
    }

    private void Start()
    {
        StartCoroutine(ParallaxRoutine());
    }

    private IEnumerator ParallaxRoutine()
    {
        while (true)
        {
            transform.position += Vector3.left * (_speed + GameManager.Instance.CurrentGameSpeed) * Time.deltaTime;
            if(_lastBackground.transform.position.x <= _initPos.x)
                transform.position = _lastBackground.transform.position;

            yield return null;
        }     
    }
}
