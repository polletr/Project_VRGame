using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1000f;
    private Rigidbody _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        Destroy(gameObject, 3f);
    }
    private void Update()
    {
        _rb.velocity = transform.forward * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
