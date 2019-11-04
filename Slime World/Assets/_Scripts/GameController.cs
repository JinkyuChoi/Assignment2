using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//2019-11-03 by Jinkyu Choi
public class GameController : MonoBehaviour
{
    [Header("Properties")]
    public GameObject player;

    [Header("Scoreboard")]
    public GameObject scoreBoard;
    [SerializeField]
    private int _score;
    [SerializeField]
    private int _hitpoint;


    [Header("UI Control")]
    public Text scoreLabel;
    public Text hitpointLabel;
    public Text congratulations;
    public Text currentScore;
    public GameObject restartButton;

    [Header("Game Control")]
    public bool gameEnd;
    public Transform respawnPoint;

    public int Hitpoint
    {
        get
        {
            return _hitpoint;
        }

        set
        {
            _hitpoint = value;
            scoreBoard.GetComponent<Scoreboard>().hitpoint = _hitpoint;

            hitpointLabel.text = "HP: " + _hitpoint.ToString();

            if (_hitpoint <= 0)
            {
                //If the player dies it will reduce 1000 score and reset the player position
                Score -= 1000;
                Reset();
                Hitpoint = 10;
            }
        }
    }
    public int Score
    {
        get
        {
            return _score;
        }

        set
        {
            //This will change score in scoreBoard every time new score is assigned
            _score = value;
            scoreBoard.GetComponent<Scoreboard>().score = _score;

            scoreLabel.text = "Score: " + _score.ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitialSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameEnd)
        {
            GameEnd();
        }
    }

    //code from Tom Tsiliopoulos "Mail Pilot"
    //This will do Initial Setting before game start
    //Like hiding all the GameOver text and set score to 0
    void InitialSetting()
    {
        scoreLabel.enabled = true;
        hitpointLabel.enabled = true;
        congratulations.enabled = false;
        currentScore.enabled = false;
        restartButton.SetActive(false);
        Score = 0;
        Hitpoint = 10;
    }

    //code from Tom Tsiliopoulos "Mail Pilot"
    //This will Show your final score and congratulation text
    //It will also disable the text during the game
    void GameEnd()
    {
        congratulations.enabled = true;
        currentScore.text = "Your Score : " + scoreBoard.GetComponent<Scoreboard>().score.ToString();
        currentScore.enabled = true;
        restartButton.SetActive(true);

        scoreLabel.enabled = false;
        hitpointLabel.enabled = false;
    }

    //code from Tom Tsiliopoulos "In class"
    //This will reset the player
    private void Reset()
    {
        player.transform.position = respawnPoint.position;
    }

    //code from Tom Tsiliopoulos "Mail Pilot"
    //This will load Scene agian if restart is pressed
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }
}
