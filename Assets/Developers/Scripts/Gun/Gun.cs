using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private InputHandler _inputHandler;
    private bool _inputTriggerd;
    private bool _cooldownActive;

    private void Start()
    {
        _inputHandler = InputHandler.Instance;
    }

    private void Update()
    {
        CheckInput();
        StartShoot();
    }

    private void CheckInput()
    {
        Debug.Log(_inputHandler.shootTriggered);
        _inputTriggerd = _inputHandler.shootTriggered;
    }

    private void StartShoot()
    {
        if (_cooldownActive == false)
        {
            if (_inputTriggerd == true)
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                StartCoroutine(ShootCooldown());
            }
        }
    }

    private IEnumerator ShootCooldown()
    {
        _cooldownActive = true;
        yield return new WaitForSeconds(0.5f);
        _cooldownActive = false;
    }
}
