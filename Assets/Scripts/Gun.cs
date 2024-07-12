using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class Gun : MonoBehaviour
{
    [SerializeField, Header("Shoot Action")]
    public InputActionReference shoot;

    [SerializeField, Header("Bullet Action")]
    public GameObject bulletPrefab;
    [SerializeField]
    private Transform _bulletSpawnPoint;

    [SerializeField, Header("Aim Settings")]
    private LineRenderer _lineRenderer;
    [SerializeField]
    private float _maxAimDistance = 100f;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float flashRate = 0.3f;
    [SerializeField]
    private GameObject flash;

    int shootAnimation = Animator.StringToHash("Shoot");

    float _timer;

    void OnEnable()
    {
        shoot.action.performed += ctx => Shoot();
    }
    private void OnDisable()
    {
        shoot.action.performed -= ctx => Shoot();
    }
    void Update()
    {
        UpdateAimLine();
    }

    private void Shoot()
    {
        StartCoroutine(FlashShoot());
        animator.Play(shootAnimation);
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

    private IEnumerator FlashShoot()
    {
        CountdownTimer timer = new CountdownTimer(flashRate);
        timer.Start();
        flash.SetActive(true);

        while (timer.IsRunning)
        {
            timer.Tick(Time.deltaTime);
            yield return null;
        }
        flash.SetActive(false);
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
