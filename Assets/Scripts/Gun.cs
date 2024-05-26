using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField,Header("Shoot Action")]
    public InputActionReference shoot;
    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField,Header("Bullet Action")]
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform _bulletSpawnPoint;

    float _timer;
    bool isShooting;
    void Update()
    {
        isShooting = (shoot.action.ReadValue<float>() == 1);
        _timer += Time.deltaTime;
        if (isShooting && _timer >= _fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            _timer = 0;
            Debug.Log("Shoot");
        }

    }
}
