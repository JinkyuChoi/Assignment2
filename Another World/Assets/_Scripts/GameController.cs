using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Scoreboard")]
    [SerializeField]
    private int _score;

    public GameObject scoreBoard;

    [Header("UI Control")]

    public Text scoreLabel;

    public Text congratulations;
    public Text currentScore;
    public GameObject restartButton;

    public bool gameEnd;
    // public properties
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
        restartButton = GameObject.Find("Button");
        scoreLabel.enabled = true;
        congratulations.enabled = false;
        currentScore.enabled = false;
        restartButton.SetActive(false);
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameEnd)
        {
            GameEnd();
        }
    }

    void GameEnd()
    {
        congratulations.enabled = true;
        currentScore.text = "Your Score : " + scoreBoard.GetComponent<Scoreboard>().score.ToString();
        currentScore.enabled = true;
        restartButton.SetActive(true);

        scoreLabel.enabled = false;
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }
}
