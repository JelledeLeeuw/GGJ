using System;
using System.Collections;
using UnityEngine;

namespace Developers.Scripts
{
    public class BouncePad : MonoBehaviour
    {
        [SerializeField]
        private float bounceForce = 10f;

        [SerializeField]
        private float Delay; // delays reactivation.

        [SerializeField]
        private ForceMode _forceMode;

        private bool _delayActive;

        private void OnTriggerEnter(Collider other)
        {
            print("trigger");
            if (other.gameObject.CompareTag("Player"))
            {
                if (_delayActive)
                    return;
                print("yus");
                other.attachedRigidbody.AddForce(Vector3.up * bounceForce, _forceMode);
                StartCoroutine(DelayEnum());    
            }
        }


        private IEnumerator DelayEnum()
        {
            _delayActive = true;
            yield return new WaitForSeconds(Delay);
            _delayActive = false;
        }
    }
}
