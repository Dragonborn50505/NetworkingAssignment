using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private float m_health;

    private bool m_isAlive = true;
    public bool isAlive
    {
        get { return m_isAlive; }
    }

    private float m_damange;

    public float Damange
    {
        get { return m_damange; }
        set { m_damange = value; }
    }


    public void TakeDamange()
    {
        if (!IsServer) return;

        m_health -= m_damange;

        if (m_health > 0) return;

        //DiedServerRpc();
        DisableDeathRPC();
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void DiedServerRpc()
    {
        //gameObject.SetActive(false);
        //NetworkObject.Despawn(gameObject);
        //NetworkObject.Destroy(gameObject);
        this.gameObject.GetComponent<NetworkObject>().Despawn();
        Destroy(this.gameObject);
       
    }

    [Rpc(SendTo.Everyone)]
    public void DisableDeathRPC()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        int nrOfChildren = gameObject.transform.childCount;
        for (int i = 0; i < nrOfChildren; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        m_isAlive = false;
    }

    [Rpc(SendTo.Everyone)]
    public void ResurrectRPC()
    {
        Debug.Log("ResurrectRPC");
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        int nrOfChildren = gameObject.transform.childCount;
        for (int i = 0; i < nrOfChildren; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        m_isAlive = true;
    }
}
