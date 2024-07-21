using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class GameInfo : NetworkBehaviour
{

    [System.Serializable]
    public struct AllPlayerInfo
    {
        public int playerId;
        public GameObject playerObj;
        public string playerName;
        public int playerKillCount;
        public int playerDeathCount;

    }
    //public PlayerInfo[] playerInfo;
    public List<AllPlayerInfo> playerInfos = new List<AllPlayerInfo>();
    public List<GameObject> allPlayers = new List<GameObject>();
    public static GameInfo instance;
    ////GunInfo
    //public enum GunType { AK47, Pistol, Bazzuka, Sniper }
    //[System.Serializable]
    //public struct GunInformation
    //{
    //    public GunType gunType;
    //    public float damage;
    //    public float range;
    //    public float fireRate;
    //    public float reloadTime;
    //}
    //public GunInformation gunInfo;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    [ServerRpc]
    public void AddPlayerServerRPC(ulong id)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(id, out var networkedClient))
        {
            GameObject _playerObj = networkedClient.PlayerObject.gameObject;

            if (_playerObj != null)
            {
                PlayerInfo playerInfo = _playerObj.GetComponent<PlayerInfo>();

                if (playerInfo != null && playerInfo.playerNameInputField != null)
                {
                    string playerName = playerInfo.playerNameInputField.text;

                    AllPlayerInfo newPlayerInfo = new AllPlayerInfo();
                    newPlayerInfo.playerObj = _playerObj;
                    newPlayerInfo.playerName = playerName;

                    playerInfos.Add(newPlayerInfo);
                }
                else
                {
                    Debug.LogError("PlayerInfo component or playerNameInputField is null.");
                }
            }
            else
            {
                Debug.LogError("Player GameObject is null.");
            }
        }
        else
        {
            Debug.LogError("Networked client with ID " + id + " not found.");
        }
    }


    [ClientRpc]
    public void UpdatePlayerListClientRpc()
    {

    }
}
