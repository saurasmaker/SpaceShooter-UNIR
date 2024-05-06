using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    private ProjectileController _mainProjectile;
    private ObjectPool<ProjectileController> _projectilesPool;
    public ObjectPool<ProjectileController> ProjectilesPool
    {
        get {
            if (_projectilesPool == null)
                _projectilesPool = new ObjectPool<ProjectileController>(CreateProjectile, OnGetProjectile, OnReleaseProjectile, OnDestroyProjectile);

            return _projectilesPool;
        }
    }


    private ProjectileController CreateProjectile()
    {
        ProjectileController newProjectile = Instantiate(_mainProjectile);
        newProjectile.ProjectilePool = _projectilesPool;
        newProjectile.gameObject.SetActive(false);

        return newProjectile;
    }

    private void OnGetProjectile(ProjectileController controller)
    {
        controller.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(ProjectileController controller)
    {
        controller.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(ProjectileController controller)
    {
        Destroy(controller.gameObject);
    }
}
