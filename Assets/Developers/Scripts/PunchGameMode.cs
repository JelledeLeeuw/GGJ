using UnityEngine;

public class PunchGameMode : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
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
                rb.AddForce(knockbackDirection * 10f, ForceMode.Impulse);
            }
        }
    }
}
