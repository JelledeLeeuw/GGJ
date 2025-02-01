using System;
using System.Collections;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private GameObject checkpointsParent;

    public GameObject[] checkpointsArray;

    private Vector3 startingPoint;

    private const string save_Checkpoint_Index = "Last_Checkpoint_Index";

    private Tag tagScript;
    private TagManager tagManager;

    private void Awake()
    {
        LoadCheckpoints();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        int savedCheckpointIndex = -1;
        savedCheckpointIndex = PlayerPrefs.GetInt(save_Checkpoint_Index, -1);

        if(savedCheckpointIndex != -1)
        {
            startingPoint = checkpointsArray[savedCheckpointIndex].transform.position;
            RespawnPLayer();
        }
        else
        {
            startingPoint = gameObject.transform.position;
        }
    }

    private void Update()
    {
        if(transform.position.y <= -1)
        {
            RespawnPLayer();
        }
    }

    private void RespawnPLayer()
    {
        gameObject.transform.position = startingPoint;
        rb.angularVelocity = Vector3.zero;
        for (int i = 0; i < tagManager.Players.Count; i++)
        {
            tagManager.Players[i].GetComponent<Tag>().tagged.Value = false;
        }
        tagScript.tagged.Value = true;
    }

    private void LoadCheckpoints()
    {
        checkpointsArray = new GameObject[checkpointsParent.transform.childCount];

        int index = 0;

        foreach(Transform singleCheckpoint in checkpointsParent.transform)
        {
            checkpointsArray[index] = singleCheckpoint.gameObject;
            index++;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            int checkPointIndex = -1;
            checkPointIndex = Array.FindIndex(checkpointsArray, match => match == other.gameObject);

            if(checkPointIndex != -1)
            {
                PlayerPrefs.SetInt(save_Checkpoint_Index, checkPointIndex);
                startingPoint = other.gameObject.transform.position;
                other.gameObject.SetActive(false);
            }
        }
    }
}
