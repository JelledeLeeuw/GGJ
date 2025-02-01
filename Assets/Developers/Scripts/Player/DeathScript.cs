using System;
using System.Collections;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 startingPoint = new Vector3(-22, 7, -14);

    private Tag tagScript;
    
    private TagManager tagManager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(transform.position.y <= -1)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        gameObject.transform.position = startingPoint;
        rb.angularVelocity = Vector3.zero;
        for (int i = 0; i < tagManager.Players.Count; i++)
        {
            tagManager.Players[i].GetComponent<Tag>().tagged.Value = false;
        }
        tagScript.tagged.Value = true;
    }
}
