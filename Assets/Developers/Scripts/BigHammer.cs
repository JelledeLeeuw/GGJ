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
        Debug.Log(_rotation);
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, _target,ref r, 0.5f);
        transform.rotation = Quaternion.Euler(0,-90,_angle);
    }
}
