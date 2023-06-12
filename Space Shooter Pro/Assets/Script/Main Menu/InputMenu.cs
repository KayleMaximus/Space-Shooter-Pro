using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InputMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _InputField; 

    public void GetPlayerName()
    {
        string Playername = _InputField.GetComponent<TMP_InputField>().text;
        PlayerPrefs.SetString("PlayerName", Playername);
        SceneManager.LoadScene(1);
    }
}
