using UnityEngine;

public class PunchGameMode : MonoBehaviour
{
    [SerializeField]
    private float force = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        bool critHit = Random.Range(0f, 1f) > 0.5f;
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Get the direction **away from the collision point**
                Vector3 knockbackDirection = (
                    collision.transform.position - transform.position
                ).normalized;

                // Apply force in that direction
                rb.AddForce(knockbackDirection * (critHit ? force * 4 : force), ForceMode.Impulse);
            }
        }
    }
}
