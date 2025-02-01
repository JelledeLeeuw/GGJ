using System;
using UnityEngine;

public class PunchGameMode : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other
                .gameObject.GetComponent<Rigidbody>()
                .AddForce(other.transform.forward * -100, ForceMode.Impulse);
        }
    }
}
