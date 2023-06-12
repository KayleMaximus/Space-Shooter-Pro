using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private GameObject _InputMenu;
    [SerializeField]
    private GameObject _MainMenu;
    [SerializeField]
    private GameObject _ScoreBoard;

    private void Start()
    {
        _InputMenu.SetActive(false);
        _MainMenu.SetActive(true);
        audioMixer.SetFloat("Volume", 0f);
    }

    public void LoadGame()
    {
        PlayerPrefs.SetString("PlayerName", "");
        if (PlayerPrefs.GetString("PlayerName","") != "")
        {
            SceneManager.LoadScene(1);//Game Scene //Load = Index sẽ nhanh hơn load = String //SceneManager.LoadScene("Game");
                                      //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            _InputMenu.SetActive(true);
            _MainMenu.SetActive(false);
        }

    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void ScoreBoard()
    {
        _ScoreBoard.SetActive(true);
        _MainMenu.SetActive(false);
    }
}
