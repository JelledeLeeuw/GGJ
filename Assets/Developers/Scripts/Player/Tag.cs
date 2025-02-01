using Unity.Netcode;
using UnityEngine;

public class Tag : NetworkBehaviour
{
    [SerializeField] private GameObject tagLight;
    public NetworkVariable<bool> tagged = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private float tagCooldown = 1.5f; // Cooldown time in seconds
    private float lastTagTime = 0f;   // Tracks last tag time

    private void Start()
    {
        tagged.OnValueChanged += OnTagChanged;  
    }

    private void OnDestroy()
    {
        tagged.OnValueChanged -= OnTagChanged;  
    }

    private void OnTagChanged(bool oldValue, bool newValue)
    {
        tagLight.SetActive(newValue);  
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsServer) return;  

        if (tagged.Value && other.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastTagTime < tagCooldown) return; // Prevent instant swaps

            Debug.Log($"Tagging {other.gameObject.name}");
            lastTagTime = Time.time;  // Update last tag time
            TransferTag(other.gameObject);
        }
    }

    private void TransferTag(GameObject newTagger)
    {
        Tag newTagScript = newTagger.GetComponent<Tag>();
        if (newTagScript == null) return;

        tagged.Value = false;
        newTagScript.SetTaggedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetTaggedServerRpc()
    {
        tagged.Value = true;
        Debug.Log($"{gameObject.name} is now tagged!");
        lastTagTime = Time.time;  // Ensure cooldown applies to the new tagger
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        tagLight.SetActive(tagged.Value);  
    }
}