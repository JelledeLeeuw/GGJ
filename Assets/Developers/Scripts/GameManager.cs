using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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
            GetComponent<TagManager>().StartGame();
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
    }

    public enum GameState
    {
        Joining,
        Starting,
        Playing
    }
}
