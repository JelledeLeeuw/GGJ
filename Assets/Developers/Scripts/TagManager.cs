using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TagManager : NetworkBehaviour
{
    public List<GameObject> Players = new List<GameObject>();

    private void Update()
    {
        Players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void StartGame()
    {
        int rng = UnityEngine.Random.Range(0, Players.Count + 1);

        Players[rng].GetComponent<Tag>().tagged.Value = true;
        
        Cursor.lockState = CursorLockMode.Locked;
    }
}
