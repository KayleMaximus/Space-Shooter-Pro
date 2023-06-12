using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform _entryContainer;
    private Transform _entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Start()
    {

    }

    private void Awake()
    {

        _entryContainer = transform.Find("highscoreEntryContainer");
        _entryTemplate = _entryContainer.Find("highscoreEntryTemplate");

        _entryTemplate.gameObject.SetActive(false);

        //Save New Player Score
        if (PlayerPrefs.GetInt("PlayerScore", 0) != 0 && PlayerPrefs.GetString("PlayerName") != null)
        {
            AddHighscoreEntry(PlayerPrefs.GetInt("PlayerScore", 0), PlayerPrefs.GetString("PlayerName", "AAA"));
        }

        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);


        //xoa sach danh sach
        /*        highscores.highscoreEntryList.Clear();
                string json = JsonUtility.ToJson(highscores);
                PlayerPrefs.SetString("highscoreTable", json);
                PlayerPrefs.Save();*/

        //Sort The List
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        //Limit 10
        if (highscores.highscoreEntryList.Count > 10)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
            {
                highscores.highscoreEntryList.RemoveAt(10);
            }
        }

        //Creat UI
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, _entryContainer, highscoreEntryTransformList);
        }
    }

    public void AddHighscoreEntry(int score, string name)
    {
        //Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry(score, name);

        //Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //Limit 10
        if (highscores.highscoreEntryList.Count > 10)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
            {
                highscores.highscoreEntryList.RemoveAt(10);
            }
        }

        //Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> tranformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(_entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * tranformList.Count);
        entryRectTransform.gameObject.SetActive(true);

        int rank = tranformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

        if (rank == 1)
        {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.yellow;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.yellow;
        }

        tranformList.Add(entryTransform);
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;

        public HighscoreEntry(int score, string name)
        {
            this.score = score;
            this.name = name;
        }
    }


}
