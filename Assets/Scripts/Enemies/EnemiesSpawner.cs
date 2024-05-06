using System.Collections;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField]
    EnemiesPool _asteroidsPool;
    
    [SerializeField]
    float _minTime = 3f, _maxTime = 5f, _incrementalConst = 1f;


    private void Awake()
    {
        if(_asteroidsPool != null)
            _asteroidsPool = GetComponent<EnemiesPool>();
    }

    void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }


    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            _asteroidsPool.MainAsteroidsPool.Get();
            yield return new WaitForSeconds(Random.Range(_minTime, _maxTime) - _incrementalConst * (GameManager.Instance.CurrentGameSpeed / GameManager.Instance.MaxGameSpeed));
        }
    }
}
