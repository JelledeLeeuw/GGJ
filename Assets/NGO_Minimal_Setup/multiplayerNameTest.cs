using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Multiplayer;

public class multiplayerNameTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AuthenticationService.Instance.UpdatePlayerNameAsync("Test");
    }
}
