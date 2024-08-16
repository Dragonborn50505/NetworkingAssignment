using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{

    [SerializeField] private GameObject m_blueTeamBox;
    [SerializeField] private GameObject m_redTeamBox;

    [SerializeField] private TextMeshProUGUI m_blueTeamTextList;
    [SerializeField] private TextMeshProUGUI m_redTeamTextList;

    private Player playerScript;

    public List<GameObject> m_blueTeam;
    public List<GameObject> m_redTeam;

    private TeamAssigner m_blueTeamBoxScript;
    private TeamAssigner m_redTeamBoxScript;

    private bool m_IsInLobby = true;
    public bool IsInLobby
    {
        get { return m_IsInLobby; }
    }

    // Start is called before the first frame updatew aw
    void Start()
    {
        m_blueTeamBoxScript = m_blueTeamBox.GetComponent<TeamAssigner>();
        m_redTeamBoxScript = m_redTeamBox.GetComponent<TeamAssigner>();
    }

    // Update is called once per frame
    void Update()
    {

        if (m_IsInLobby)
        {
            m_blueTeam = m_blueTeamBoxScript.m_teamMembers;
            m_redTeam = m_redTeamBoxScript.m_teamMembers;

            m_blueTeamTextList.text = m_blueTeam.Count.ToString();
            m_redTeamTextList.text = m_redTeam.Count.ToString();
            StartGame();
        }
    }

    private void StartGame()
    {
        if ((m_blueTeam.Count >= 1) && 
            (m_redTeam.Count >= 1) && 
            (m_blueTeam.Count == m_redTeam.Count + 1 || 
            m_blueTeam.Count == m_redTeam.Count || 
            m_blueTeam.Count == m_redTeam.Count - 1))
        {
            m_IsInLobby = false;

           

            GameObject TeamTransferObj = GameObject.FindGameObjectWithTag("TeamTransfer");
            TeamTransferObj.GetComponent<ScoringManagment>().SaveTeamsRpc();

            //SceneManager.LoadScene(1);


            Invoke("LoadScene", 3);


        }
        
    }
    private void LoadScene()
    {
        string SceneName = "GameScene";
        NetworkManager.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }


}
