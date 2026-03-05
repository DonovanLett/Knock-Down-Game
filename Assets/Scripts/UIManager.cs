using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _openingText, _finalScoreText01, _finalScoreText02, _restartText;
    [SerializeField]
    private bool _firstRound;
    [SerializeField]
    private TMP_Text _timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRoundStart()
    {
        if(_openingText.IsActive() == true)
        {
            _openingText.gameObject.SetActive(false);
        }
        else if (_restartText.IsActive() == true)
        {
            _restartText.gameObject.SetActive(false);
        }
        _timer.gameObject.SetActive(true);
    }

    public void Timer(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        string timeString = minutes + ":" + seconds.ToString("00");

        _timer.text = timeString;
    }

    public void OnRoundEnd()
    {
        _timer.gameObject.SetActive(false);
    }

    public void EndOfRoundText(int finalScore, int highScore)
    {
        if (finalScore == 1)
        {
            _finalScoreText01.text = "You knocked 1 bottle over.";
        }
        else
        {
          _finalScoreText01.text = "You knocked " + finalScore.ToString() + " bottles over.";
        }


        if (finalScore == 0 || finalScore == 1)
        {
            _finalScoreText02.text = ":/";
        }
        else if (_firstRound == true)
        {
            _finalScoreText02.text = "Impressive start.";
        }
        else if (finalScore <= highScore)
        {
            int difference = ((highScore + 1) - finalScore);

            _finalScoreText02.text = "That's " + difference.ToString() + " shy of a new high score.";
        }
        else if (finalScore > highScore)
        {
            _finalScoreText02.text = "New high score!";
        }

        if(_firstRound == true)
        {
            _firstRound = false;
        }
    }

    
}
