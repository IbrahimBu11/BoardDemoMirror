using System;
using System.Collections;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
   public int index;
   public int id;
   [SyncVar (hook = nameof(OnPlayerScoreUpdated))] [SerializeField] private int _score;
   [SyncVar (hook = nameof(OnSyncPlayedId))] [SerializeField] private int _id;

   private void Awake()
   {
      GameEvents.OnPlayerScoreUpdateLocal += OnPlayerScoreReceived;
      _id = (int)netId;

      if (isLocalPlayer)
      {
         Board.localPlayerID = id;
      }
         
   }
   private void OnDestroy()
   {
      GameEvents.OnPlayerScoreUpdateLocal -= OnPlayerScoreReceived;
   }

   private void OnSyncPlayedId(int oldId, int newId)
   {
      id = newId;
   }

   private void OnPlayerScoreReceived(int i, int score)
   {
      if (isServer)
      {
         _score += score;
         GameEvents.OnPlayerScoreUpdateSync?.Invoke(id, _score);
      }
      else
         TellServerScore(score);
      
   }

   [Command]
   private void TellServerScore(int score)
   {
      _score += score;
   }

   

   public override void OnStartServer()
   {
      id = (int) GetComponent<NetworkIdentity>().netId;
      print("Server Start Called");
      //base.OnStartServer();
      StartCoroutine(WaitAndLaunch());
   }

   private IEnumerator WaitAndLaunch()
   {
      yield return new WaitForSeconds(1);
      GameEvents.OnPlayerJoinIndex.Invoke(id,index);
   }

   private void OnPlayerScoreUpdated(int oldScore, int newScore)
   {
      GameEvents.OnPlayerScoreUpdateSync?.Invoke(id, newScore);
   }
}


