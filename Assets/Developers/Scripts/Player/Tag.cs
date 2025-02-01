using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class Tag : NetworkBehaviour
{
    [SerializeField]
    private GameObject tagLight;

    public ulong taggedPlayer;

    private float tagCooldown = 1.5f; // Cooldown time in seconds
    private float lastTagTime = 0f; // Tracks last tag time

    private TagManager tagManager;

    private void Start()
    {
        taggedPlayer = 99999;
        tagManager = FindFirstObjectByType<TagManager>();
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (taggedPlayer == OwnerClientId && other.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastTagTime < tagCooldown)
                return; // Prevent instant swaps

            Debug.Log($"Tagging {other.gameObject.name}");
            lastTagTime = Time.time; // Update last tag time
            TransferTag(other.gameObject);
        }
    }

    private void TransferTag(GameObject newTagger)
    {
        Tag newTagScript = newTagger.GetComponent<Tag>();
        if (newTagScript == null)
            return;
        newTagScript.RecieveTag();
    }

    public void RecieveTag()
    {
        if (tagManager == null)
        {
            Debug.LogWarning("TagManager not found.");
            tagManager = FindFirstObjectByType<TagManager>();
        }
        tagManager.SetTaggedPlayerRpc(OwnerClientId);
        SetLightRpc();
    }

    public void SetLightRpc()
    {
        tagLight.SetActive(taggedPlayer == OwnerClientId);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        // tagLight.SetActive(tagged.Value);
    }
}
