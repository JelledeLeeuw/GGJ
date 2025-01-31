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
                print("yus");
                other.attachedRigidbody.AddForce(Vector3.up * bounceForce, _forceMode);
            }
        }


        // private IEnumerator OnCollisionExit(Collision other)
        // {
        //     _delayActive = true;
        //     yield return new WaitForSeconds(Delay);
        //     
        // }
    }
}
