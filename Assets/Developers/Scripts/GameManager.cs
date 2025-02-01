using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Developers.Scripts
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }
        public GameState gameState;
        private int _amountPlayers;
        private List<PlayerMovement> _players = new();
        private int _playersReady;

        [SerializeField]
        private GameObject connectPrefab;

        [SerializeField]
        private Button startButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                return;
            }
            Destroy(gameObject);
        }

        private void Start()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            startButton.onClick.AddListener(StartGame);
            startButton.interactable = false;
        }

        private void StartGame()
        {
            if (!HasAuthority)
            {
                Debug.LogWarning("Only the session owner can start the game.");
                return;
            }

            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                GetComponent<TagManager>().StartGame();
            }
            StartGameRpc();
        }

        [Rpc(SendTo.Everyone, Delivery = RpcDelivery.Reliable)]
        private void StartGameRpc()
        {
            gameState = GameState.Starting;
            connectPrefab.SetActive(false);

            foreach (PlayerMovement player in _players)
                player.GameStartRpc();
        }

        public void AddPlayer(PlayerMovement player)
        {
            startButton.interactable = true;
            _players.Add(player);
            _amountPlayers++;
        }

        public void PlayerReady()
        {
            _playersReady++;
            if (_playersReady == _amountPlayers)
                GameStartedRpc();
        }

        [Rpc(SendTo.Everyone, Delivery = RpcDelivery.Reliable)]
        private void GameStartedRpc()
        {
            gameState = GameState.Playing;
        }

        public int maxPlayers = 4; // Change this to your desired max player count

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response
        )
        {
            if (NetworkManager.Singleton.ConnectedClients.Count >= 10)
            {
                Debug.Log("Max player limit reached. Rejecting connection.");
                response.Approved = false; // Reject connection if max players reached
            }
            else
            {
                response.Approved = true;
            }

            response.Pending = false;
        }
    }

    public enum GameState
    {
        Joining,
        Starting,
        Playing
    }
}
