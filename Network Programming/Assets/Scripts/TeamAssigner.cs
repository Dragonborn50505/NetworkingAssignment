using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamAssigner : NetworkBehaviour
{
    [SerializeField] private string m_team;


    public List<GameObject> m_teamMembers;
    private Player playerScript;
    private GameObject GameManager;

    private void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        m_teamMembers.Add(collision.gameObject);

        collision.gameObject.GetComponent<Player>().Team = m_team;


        //Debug.Log($"Added Player: {collision.gameObject.name}");
        //Debug.Log($"Players in that team: {m_teamMembers.Count}");


        /*
        for (int i = 0; i < m_teamMembers.Count; i++)
        {
            Debug.Log(m_teamMembers[i].name);
        }
        */
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (!GameManager.GetComponent<GameManager>().IsInLobby) return;


        collision.gameObject.GetComponent<Player>().Team = null;
        m_teamMembers.Remove(collision.gameObject);


    }
    

}
