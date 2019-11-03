﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Scoreboard")]
    [SerializeField]
    private int _lives;

    [SerializeField]
    private int _score;

    public Text livesLabel;
    public Text scoreLabel;
    public Text highScoreLabel;

    public GameObject scoreBoard;

    private int currentLives;
    private int currentScore;

    [Header("UI Control")]
    public GameObject startLabel;
    public GameObject startButton;
    public GameObject endLabel;
    public GameObject restartButton;

    // public properties
    public int Lives
    {
        get
        {
            return _lives;
        }

        set
        {
            _lives = value;
            scoreBoard.GetComponent<Scoreboard>().lives = _lives;

            if (_lives < 1)
            {

                //SceneManager.LoadScene("End");
            }
            else
            {
                livesLabel.text = "Lives: " + _lives.ToString();
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
            _score = value;
            scoreBoard.GetComponent<Scoreboard>().score = _score;


            if (scoreBoard.GetComponent<Scoreboard>().highScore < _score)
            {
                scoreBoard.GetComponent<Scoreboard>().highScore = _score;
            }
            scoreLabel.text = "Score: " + _score.ToString();

            //if (_score >= 500 && SceneManager.GetActiveScene().name != "Level2")
            //{
            //    SceneManager.LoadScene("Level2");
            //}
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}