using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoringManagment : NetworkBehaviour
{
    public List<GameObject> m_blueTeamSaved;
    public List<GameObject> m_redTeamSaved;

    private GameObject TeamTransferObj;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SaveTeamsRpc()
    {

        TeamTransferObj = GameObject.FindGameObjectWithTag("GameManager");

        m_blueTeamSaved = TeamTransferObj.GetComponent<GameManager>().m_blueTeam;
        m_redTeamSaved = TeamTransferObj.GetComponent<GameManager>().m_redTeam;

    }
}
