using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatScript : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI m_messages;
    [SerializeField] private TextMeshProUGUI m_input;


    public void CallMessagesRPC()
    {
        string message = m_input.text;
        SendMessageRPC(message);
    }

    [Rpc(SendTo.Everyone)]
    public void SendMessageRPC(string message)
    {
        m_messages.text = $"{message}";
    }
}
