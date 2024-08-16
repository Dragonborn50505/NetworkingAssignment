using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Player : NetworkBehaviour
{

    [SerializeField] private InputReader m_inputReader;
    [SerializeField] private GameObject m_bulletObj;
    [SerializeField] private float m_speed;

    private Vector2 m_spawnArea;
    public Vector2 SpawnArea
    {
        get { return m_spawnArea; }
        set { m_spawnArea = value; }
    }

    //private Vector2 Mousedirection;
    private Vector2 m_moveDirection;
    private Vector3 m_flatVelocity;
    private Rigidbody2D rb2D;
    private GameManager gameManager;
    private bool GameManagerIsInLobby;
    private List<GameObject> m_spawnedBullets = new List<GameObject>();

    public string m_team;
    public string Team
    {
        get { return m_team; }
        set { m_team = value; }
    }




    NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();
    NetworkVariable<Vector2> rotationValue = new NetworkVariable<Vector2>();


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (m_inputReader != null && IsLocalPlayer)
        {
            m_inputReader.MoveEvent += OnMove;
            m_inputReader.ShootEvent += ShootRpc;
        }
    }

    private void OnMove(Vector2 input)
    {
        MoveRPC(input);
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            RotatePlayerTowardsMouseRpc(mousePosition);
        }



        if (!IsServer) { return; }


        m_moveDirection = new Vector2(moveInput.Value.x, moveInput.Value.y);
        rb2D.velocity = m_moveDirection;

        //rb2D.AddForce(m_moveDirection);
        //SpeedControl();

        transform.up = rotationValue.Value;

    }

    /*
    private void FixedUpdate()
    {
        if (!IsServer) { return; }

        m_moveDirection = new Vector2(moveInput.Value.x, moveInput.Value.y);
        rb2D.velocity = m_moveDirection;
        //rb2D.AddForce(m_moveDirection.normalized * m_speed);
    }
    */

    //[ServerRpc]
    [Rpc(SendTo.Server)]
    private void ShootRpc()
    {
        //if (!IsLocalPlayer) return;
        //if (!gameObject.active) return;

        
        /*
        NetworkObject Bullet = Instantiate(m_bulletObj, transform.position, transform.rotation).GetComponent<NetworkObject>();
        //Bullet.GetComponent<Bullet>().PlayerParent = this.gameObject;
        Bullet.GetComponent<Bullet>().PlayerParentObj = this.gameObject; //awada
        Bullet.Spawn();
        */
        GameObject Bullet = Instantiate(m_bulletObj, transform.position, transform.rotation);
        m_spawnedBullets.Add(Bullet);
        Bullet.GetComponent<TestBulletScript>().Parent = this;
        Bullet.GetComponent<TestBulletScript>().ParentObj = this.gameObject;
        Bullet.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyBulletServerRpc()
    {
        GameObject TeamManager = GameObject.FindGameObjectWithTag("TeamManager");
        if (TeamManager)
        {
            Debug.Log("Team Fix");
            TeamManager.GetComponent<TeamManager>().EmptyList();
            TeamManager.GetComponent<TeamManager>().LookForAssignedTeams();

        }
        else
        {
            Debug.Log("Cant find TeamManager");
        }

        GameObject toDestroy = m_spawnedBullets[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        m_spawnedBullets.Remove(toDestroy);
        Destroy(toDestroy);
    }

    [Rpc(SendTo.Server)]
    private void MoveRPC(Vector2 data)
    {
        moveInput.Value = data;
    }

    [Rpc(SendTo.Server)]
    private void RotatePlayerTowardsMouseRpc(Vector3 mousePosition)
    {
        //if (!IsLocalPlayer) return;



        //Mousedirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        rotationValue.Value = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        //transform.up = rotationValue.Value;
    }

    private void SpeedControl()
    {
        /*
        m_flatVelocity = new Vector3(rb2D.velocity.x, rb2D.velocity.y);

        //Check if player velocity is larger than move speed and reset it to normal speed
        if (m_flatVelocity.magnitude > m_speed)
        {
            var m_limitedVelocity = m_flatVelocity.normalized * m_speed;
            rb2D.velocity = new Vector3(m_limitedVelocity.x, rb2D.velocity.y);
            rb2D.velocity.Set(0, 0);
        }
        */
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        rb2D.isKinematic = false;

        m_spawnArea.x = 0.2f;
        m_spawnArea.y = 0.2f;
        transform.position = new Vector2(Random.Range(m_spawnArea.x, -m_spawnArea.x), Random.Range(m_spawnArea.y, -m_spawnArea.y));
    }

}
