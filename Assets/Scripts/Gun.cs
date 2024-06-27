using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField,Header("Shoot Action")]
    public InputActionReference shoot;
    [SerializeField]
    private float _fireRate = 0.1f;

    [SerializeField,Header("Bullet Action")]
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform _bulletSpawnPoint;

    [SerializeField, Header("Aim Settings")]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private float _maxAimDistance = 100f;

    float _timer;
    bool isShooting;
    void Update()
    {
        isShooting = (shoot.action.ReadValue<float>() == 1);
        _timer += Time.deltaTime;
        if (isShooting && _timer >= _fireRate)
        {
            Shoot();
            _timer = 0;
        }

        UpdateAimLine();

    }

    private void Shoot()
    {
        Ray ray = new Ray(_bulletSpawnPoint.position, _bulletSpawnPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxAimDistance))
        {
            ITargetable target = hit.collider.GetComponent<ITargetable>();
            if (target != null)
            {
                target.OnHit();
                Debug.Log("Shoot");

            }
        }

    }

    private void UpdateAimLine()
    {
        Ray ray = new Ray(_bulletSpawnPoint.position, _bulletSpawnPoint.forward);
        RaycastHit hit;

        Vector3 endPosition;
        if (Physics.Raycast(ray, out hit, _maxAimDistance))
        {
            endPosition = hit.point;
        }
        else
        {
            endPosition = ray.GetPoint(_maxAimDistance);
        }

        _lineRenderer.SetPosition(0, _bulletSpawnPoint.position);
        _lineRenderer.SetPosition(1, endPosition);
    }
}
