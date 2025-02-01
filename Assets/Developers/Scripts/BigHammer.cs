using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

public class BigHammer : MonoBehaviour
{
    private float _rotation;
    private float _target = 220;
    private float r;
    private void Update()
    {
        _rotation = gameObject.transform.rotation.eulerAngles.z;
        if(_rotation > 219)
        {
            _target = 140; 
        }
        else if(_rotation < 141)
        {
            _target = 220;
        }
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, _target,ref r, 0.5f);
        transform.rotation = Quaternion.Euler(0,0,_angle);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 50, ForceMode.Impulse);
        }
    }
}
