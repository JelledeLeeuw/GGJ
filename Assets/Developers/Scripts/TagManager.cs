using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TagManager : NetworkBehaviour
{
    private List<GameObject> Players = new List<GameObject>();

    private void Update()
    {
        Players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void StartGame()
    {
        int rng = UnityEngine.Random.Range(0, Players.Count + 1);

        Players[rng].GetComponent<Tag>().RecieveTag();
    }

    [Rpc(SendTo.Everyone, Delivery = RpcDelivery.Reliable)]
    public void SetTaggedPlayerRpc(ulong player)
    {
        foreach (GameObject p in Players)
        {
            p.GetComponent<Tag>().taggedPlayer = player;
            p.GetComponent<Tag>().SetLightRpc();
        }
    }
}
