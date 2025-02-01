using System;
using Unity.VisualScripting;
using UnityEngine;

public class BigSpinner : MonoBehaviour
{
    private float _rotation;
    [SerializeField] private float rotationSpeed;
    private void FixedUpdate()
    {
        _rotation += rotationSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, _rotation, 0);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 100, ForceMode.Impulse);
        }
    }
}
