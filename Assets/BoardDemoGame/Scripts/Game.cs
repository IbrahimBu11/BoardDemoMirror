using System;
using Mirror;
using UnityEngine;


public class Game : NetworkBehaviour
{
    [SerializeField] private CanvasGroup group;
    private void Awake() 
    {
        GameEvents.OnAllPlayersCConnected += OnPlayerConnected;
    }
    private void OnDestroy()
    {
        GameEvents.OnAllPlayersCConnected -= OnPlayerConnected;
    }

    private void OnPlayerConnected()
    {
        StartGame();
        GameEvents.OnGameStart?.Invoke();
        
        print("On Game Started Server");
    }

    [ClientRpc]
    private void StartGame()
    {
        GameEvents.OnGameStart?.Invoke();
        group.alpha = 1;
        print("On Game Started Client");
    }

}
