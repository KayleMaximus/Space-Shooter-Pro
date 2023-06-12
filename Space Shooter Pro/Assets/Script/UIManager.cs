using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestText;
    [SerializeField]
    private Text _nameText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restarText;
    [SerializeField]
    private Image _liveImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    private GameManager _gameManager;

    [SerializeField]
    private int _score;

    [SerializeField]
    private int _bestScore;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _bestScore = PlayerPrefs.GetInt("HighScore", 0);
        _bestText.text = "Best: " + _bestScore;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _nameText.text = "Hero : " + PlayerPrefs.GetString("PlayerName", "Null");
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null!");
        }


    }

    public void UpdateScore()
    {
        _score += 10;
        _scoreText.text = "Score: " + _score.ToString();
    }

    public void CheckForBestScore()
    {
        //SAVE BEST SCORE
        if(_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
            _bestText.text = "Best: " + _bestScore;
            PlayerPrefs.SetInt("PlayerScore", _bestScore);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerScore", _score);
        }
    }

    public void UpdateLives(int currentLives)
    {
        _liveImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
            DisplayGameOver();
    }

    public void DisplayGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restarText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverFlicker());
    }
     
    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "GAME OVER";
        }
    }
}
