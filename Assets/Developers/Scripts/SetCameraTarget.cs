using Unity.Cinemachine;
using UnityEngine;

public class SetCameraTarget : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    // void Update()
    // {
    //
    //     player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    //     Debug.Log(player);
    //     AssignTarget();
    // }

    public void AssignTarget(Transform player)
    {
        cam.Target.LookAtTarget = player;
        cam.Target.TrackingTarget = player;
    }
}
