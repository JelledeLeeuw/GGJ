using System;
using Unity.VisualScripting;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100, ForceMode.Impulse);
        }
    }
}
