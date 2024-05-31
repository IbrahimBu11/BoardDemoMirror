using System;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class BoardControllerManager : NetworkManager
{
    public CanvasGroup canva;
    

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        
        print("Add Player working");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        //GameObject player = Instantiate(playerPrefab); 
        //player.GetComponent<Player>().index = numPlayers;
        
        //NetworkServer.AddPlayerForConnection(conn, player);
        
        canva.alpha = 1;
        print($"Serever Connect working {numPlayers}");
        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        canva.alpha = 1;
        
        GameEvents.OnRoundIndexUpdateLocal(GameData.roundIndex);
        print($"Client Workinfg {numPlayers}");
        
    }
}
