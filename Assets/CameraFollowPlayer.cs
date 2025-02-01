using Unity.Netcode;
using UnityEngine;

public class CameraFollowPlayer : NetworkBehaviour
{
    [SerializeField] private GameObject cameraLocal;
    void Start()
    {
        Instantiate(cameraLocal,transform.position,Quaternion.identity);
    }



}
