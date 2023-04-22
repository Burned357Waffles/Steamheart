using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour
{

    public Button connectButton;

    public Button disconnectButton;

    public GameObject usernameBox;

    public string username = "";

    private NetworkManager networkManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("Network Manager").GetComponent<NetworkManager>();
        disconnectButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void connectButtonPress()
    {
        username = usernameBox.GetComponent<TMP_InputField>().text;
        Debug.Log(username);
        Debug.Log("Send JoinReq");
        bool connected = networkManager.SendJoinRequest();
        if (!connected)
        {
            Debug.Log("unable to connect!");
        }

        disconnectButton.enabled = true;
    }

    public void disconnectButtonPress()
    {
        bool disconnected = networkManager.SendLeaveRequest();
        if (!disconnected)
        {
            Debug.Log("unable to disconnect!");
        }

        disconnectButton.enabled = false;
    }
    
}
