using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviour
{
    [SerializeField] private GameObject connectionScreen;
    
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private Button startHost;
    [SerializeField] private Button startClient;
    [SerializeField] private TMP_InputField ipAddress;
    [SerializeField] private TextMeshProUGUI status;

    private void Awake()
    {
        GameEvents.OnGameStart += () => connectionScreen.SetActive(false);
    }

    private void OnDestroy()
    {
        GameEvents.OnGameStart -= () => connectionScreen.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(GetPublicIPAddress());
        //ipAddress.text = GetLocalIPAddress();
        
        startHost.onClick.AddListener(StartHost);
        startClient.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        networkManager.networkAddress = ipAddress.text;
        networkManager.StartHost();
        startHost.interactable = false;
        startClient.interactable = false;
        ipAddress.interactable = false;

        status.SetText("Connected as Host");
    }

    private void StartClient()
    {
        networkManager.networkAddress = ipAddress.text;
        networkManager.StartClient();
        startHost.interactable = false;
        startClient.interactable = false;
        ipAddress.interactable = false;
        status.SetText("Connected as Client");
    }

    IEnumerator GetPublicIPAddress()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://api.ipify.org?format=json");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            status.SetText("Error fetching public IP address: " + request.error);
        }
        else
        {
            // Parse the JSON response to get the IP address
            string jsonResponse = request.downloadHandler.text;
            IPResponse ipResponse = JsonUtility.FromJson<IPResponse>(jsonResponse);
            status.SetText("Fetched Address: " + ipResponse.ip); 
            ipAddress.text = ipResponse.ip;
        }
    }
    public string GetLocalIPAddress()
    {
        string localIP = "";
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            if (string.IsNullOrEmpty(localIP))
            {
                throw new Exception("Local IP Address Not Found!");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error: " + ex.Message);
        }
        return localIP;
    }

    [System.Serializable]
    public class IPResponse
    {
        public string ip;
    }
}
