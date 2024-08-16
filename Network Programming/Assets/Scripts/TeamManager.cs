using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TeamManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ScoreText;

    public List<GameObject> m_blueTeamList;
    public List<GameObject> m_redTeamList;

    private List<GameObject> playerList;

    bool m_IsGameOver = false;

    private int m_blueTeamScore;
    private int m_redTeamScore;


    // Start is called before the first frame update
    void Start()
    {

        StartLookForAssignedTeams();

        //GameObject TeamTransferObj = GameObject.FindGameObjectWithTag("TeamTransfer");
        //m_blueTeamList = TeamTransferObj.GetComponent<ScoringManagment>().m_blueTeamSaved;  //ertet
        //m_redTeamList = TeamTransferObj.GetComponent<ScoringManagment>().m_redTeamSaved;


        for (int i = 0; i < m_blueTeamList.Count; i++)
        {
            Vector2 PointArea = new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(3.75f, 4.5f));
            m_blueTeamList[i].transform.position = PointArea;
        }
        for (int i = 0; i < m_redTeamList.Count; i++)
        {
            Vector2 PointArea = new Vector2(Random.Range(-0.7f, 0.7f), Random.Range(-0.375f, 0.45f));
            m_redTeamList[i].transform.position = PointArea;
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if (!IsServer) return;

        if (m_blueTeamList.Count <= 0 && !m_IsGameOver && m_redTeamList.Count >= 1)
        {
            m_IsGameOver = true;
            m_redTeamScore++;
            string score = $"Blue {m_blueTeamScore}  :  {m_redTeamScore} Red";
            m_ScoreText.text = score;
            Invoke("LoadScene", 3);

        }
        else if (m_redTeamList.Count <= 0 && !m_IsGameOver && m_blueTeamList.Count >= 1)
        {
            m_IsGameOver = true;
            m_blueTeamScore++;
            string score = $"Blue {m_blueTeamScore}  :  {m_redTeamScore} Red";
            m_ScoreText.text = score;
            Invoke("LoadScene", 3);
        }
        */

    }

    public void LookForAssignedTeams()
    {

        Debug.Log("LookForAssignedTeams");

        playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

        for (int i = 0; i < playerList.Count; i++)
        {
            bool PlayerAlive = playerList[i].gameObject.GetComponent<SpriteRenderer>().isVisible;
            Debug.Log($"{PlayerAlive}");

            if (PlayerAlive == true) 
            {
                string PlayerTeam;
                //GameObject.collision.gameObject.GetComponent<Player>().Team = m_team;
                PlayerTeam = playerList[i].gameObject.GetComponent<Player>().Team;

                if (PlayerTeam == "Blue")
                {
                    m_blueTeamList.Add(playerList[i]);
                }
                else if (PlayerTeam == "Red")
                {
                    m_redTeamList.Add(playerList[i]);
                }
                else
                {
                    Debug.Log("EROR!!!!!!!!!!!!!!!");
                }
            }
        }
        
        
        if (m_blueTeamList.Count <= 0 || m_redTeamList.Count <= 0)
        {
            CountScore();
        }

        
        
    }

    public void StartLookForAssignedTeams()
    {

        Debug.Log("LookForAssignedTeams");

        playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

        for (int i = 0; i < playerList.Count; i++)
        {

            string PlayerTeam;
            //GameObject.collision.gameObject.GetComponent<Player>().Team = m_team;
            PlayerTeam = playerList[i].gameObject.GetComponent<Player>().Team;

            if (PlayerTeam == "Blue")
            {
                m_blueTeamList.Add(playerList[i]);
            }
            else if (PlayerTeam == "Red")
            {
                m_redTeamList.Add(playerList[i]);
            }
            else
            {
                Debug.Log("EROR!!!!!!!!!!!!!!!");
            }
        }


        if (m_blueTeamList.Count <= 0 || m_redTeamList.Count <= 0)
        {
            CountScore();
        }



    }

    public void EmptyList()
    {
        m_blueTeamList.Clear();
        m_redTeamList.Clear();
    }

    private void PrepareForLoadScene()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<Health>().ResurrectRPC();
            Vector2 spawnArea;
            spawnArea.x = 0.2f;
            spawnArea.y = 0.2f;
            playerList[i].transform.position = new Vector2(Random.Range(spawnArea.x, -spawnArea.x), Random.Range(spawnArea.y, -spawnArea.y));
        }

        Invoke("LoadScene", 1);
    }

    private void LoadScene()
    {
        
        string SceneName = "Main";
        NetworkManager.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    private void CountScore()
    {
        if (m_blueTeamList.Count <= 0 && !m_IsGameOver && m_redTeamList.Count >= 1)
        {
            m_IsGameOver = true;
            m_redTeamScore++;
            string score = $"Blue {m_blueTeamScore}  :  {m_redTeamScore} Red";
            SendScoreRPC(score);
            //m_ScoreText.text = score;
            Invoke("PrepareForLoadScene", 3);

        }
        else if (m_redTeamList.Count <= 0 && !m_IsGameOver && m_blueTeamList.Count >= 1)
        {
            m_IsGameOver = true;
            m_blueTeamScore++;
            string score = $"Blue {m_blueTeamScore}  :  {m_redTeamScore} Red";
            SendScoreRPC(score);
            //m_ScoreText.text = score;
            Invoke("PrepareForLoadScene", 3);
        }
    }

    [Rpc(SendTo.Everyone)]
    public void SendScoreRPC(string message)
    {
        m_ScoreText.text = $"{message}";
    }
}
