using UnityEngine;

public class Target : MonoBehaviour, IDamagable
{
    public void OnHit()
    {
        Destroy(gameObject);
    }
}
