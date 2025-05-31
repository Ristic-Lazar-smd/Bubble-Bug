using UnityEngine;
using TMPro;

using Dan.Main;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _entryTextObjects;
    [SerializeField] private TMP_InputField _usernameInputField;
    int score;
    // ------------------------------------------------------------

    void Awake()
    {

    }
    private void Start()
    {
        LoadEntries();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log("T key was pressed!");
            score = ScoreManager.Instance.currentScore;
            Leaderboards.BB_Tester_Leaderboard.UploadNewEntry("testera", score, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
            }
    }

    private void LoadEntries()
    {
        Leaderboards.BB_Tester_Leaderboard.GetEntries(entries =>
        {
            foreach (var t in _entryTextObjects)
                t.text = "";

            var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
            for (int i = 0; i < length; i++)
                _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
        });
    }
    
    public void UploadEntry()
    {
        score = ScoreManager.Instance.currentScore;
        Leaderboards.BB_Tester_Leaderboard.UploadNewEntry(_usernameInputField.text, score, isSuccessful =>
        {
            if (isSuccessful)
                LoadEntries();
        });
    }
}
