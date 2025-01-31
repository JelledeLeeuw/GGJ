using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        MoveForward();
    }

    private void MoveForward()
    {
        _rb.AddForce(transform.forward * 100,ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null )
        {
            other.GetComponent<IDamagable>().OnHit();
        }
    }
}
