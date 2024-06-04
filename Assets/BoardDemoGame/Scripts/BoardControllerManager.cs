using System;
using Mirror;
using UnityEngine;

public class BoardControllerManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        // if (!conn.isReady)
        // {
        //     NetworkServer.SetClientReady(conn);
        // }
        //
        // GameObject playergo = Instantiate(playerPrefab);
        // Player player = playergo.GetComponent<Player>();
        // print($" player connected {numPlayers}");
        //
        // NetworkServer.AddPlayerForConnection(conn, playergo);
        // player.SetPlayerIDServer(numPlayers); 
        
        if (numPlayers == 2)
        {
            print("2 player connected");
            GameEvents.OnAllPlayersCConnected?.Invoke();    
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        print($"OnClient : {numPlayers}");
    }

    public override void OnServerConnect (NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        print($"OnServer : {numPlayers}");
    }
    
}
