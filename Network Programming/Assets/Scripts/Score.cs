using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : GameManager
{
    [SerializeField] private TextMeshProUGUI m_ScoreText;


    private int m_blueTeamScore;
    private int m_redTeamScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_blueTeam.Count <= 0)
        {
            m_blueTeamScore++;
        }
        else if (m_redTeam.Count <= 0)
        {
            m_redTeamScore++;
        }
        string score = $"Blue {m_blueTeamScore}  :  {m_redTeamScore} Red";
        m_ScoreText.text = score;
    }
}
