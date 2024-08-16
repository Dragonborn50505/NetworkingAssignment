using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] float m_health;

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

        DiedServerRpc();
        
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
}
