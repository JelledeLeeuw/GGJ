using UnityEngine;

public class BigSpinner : MonoBehaviour
{
    private float _rotation;
    private void FixedUpdate()
    {
        _rotation++;
        transform.rotation = Quaternion.Euler(0, _rotation,0);
    }
}
