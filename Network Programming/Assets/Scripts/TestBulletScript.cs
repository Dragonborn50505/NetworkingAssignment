using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TestBulletScript : NetworkBehaviour
{

    private Player parent;
    public Player Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    private GameObject parentObj;
    public GameObject ParentObj
    {
        get { return parentObj; }
        set { parentObj = value; }
    }

    [SerializeField] private float TimeToDestroy;
    [SerializeField] private float m_damange;
    [SerializeField] private float m_bulletSpeed = 2;


    private Health HealthScript;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D BulletRb = gameObject.GetComponent<Rigidbody2D>();
        BulletRb.AddForce(transform.up * m_bulletSpeed, ForceMode2D.Impulse);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == parentObj) return;


        if (collision.gameObject.CompareTag("Player"))
        {

            Player m_enemy = collision.gameObject.GetComponent<Player>();

            HealthScript = m_enemy.GetComponent<Health>();

            HealthScript.Damange = m_damange;
            HealthScript.TakeDamange();

            //Take Damange to write
        }

        if (!parent) return;
        parent.DestroyBulletServerRpc();
        //NetworkObject.Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!IsServer) return;


        TimeToDestroy -= Time.deltaTime;
        if (TimeToDestroy <= 0)
        {
            //NetworkObject.Despawn(gameObject);
            //NetworkObject.Destroy(gameObject);
            //this.gameObject.GetComponent<NetworkObject>().Despawn();
            //Destroy(this.gameObject);'

            parent.DestroyBulletServerRpc();
        }
    }
}
