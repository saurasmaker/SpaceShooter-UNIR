using UnityEngine;
using UnityEngine.Pool;

public class EnemiesPool : MonoBehaviour
{
    [SerializeField]
    private EnemyManager _asteroidRandomPrefab;
    //[SerializeField]
    //private int _maxSize = 10;
    [SerializeField]
    float _xOffset = 2f;


    private ObjectPool<EnemyManager> _asteroidsPool;
    public ObjectPool<EnemyManager> MainAsteroidsPool
    {
        get
        {
            _asteroidsPool ??= new ObjectPool<EnemyManager>(CreateAsteroid, OnGetAsteroid, OnReleaseAsteroid, OnDestroyAsteroid);
            return _asteroidsPool;
        }
    }


    private EnemyManager CreateAsteroid()
    {
        EnemyManager newAsteroid = Instantiate(_asteroidRandomPrefab);
        newAsteroid.AsteroidsPool = _asteroidsPool;
        newAsteroid.gameObject.SetActive(false);

        return newAsteroid;
    }

    private void OnGetAsteroid(EnemyManager controller)
    {
        controller.transform.position = new Vector3(GameManager.Instance.ScreenBounds.x + _xOffset, Random.Range(GameManager.Instance.ScreenBounds.y / -2, GameManager.Instance.ScreenBounds.y / 2));
        controller.gameObject.SetActive(true);
    }

    private void OnReleaseAsteroid(EnemyManager controller)
    {
        controller.gameObject.SetActive(false);
    }

    private void OnDestroyAsteroid(EnemyManager controller)
    {
        Destroy(controller.gameObject);
    }
}
