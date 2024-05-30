using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMatchButton : MonoBehaviour
{
    private DatabaseReference databaseRef;

    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.GetReference("users");
    }

    public void OnExitMatchClicked()
    {
        string user1ID = MatchSceneManager.User1ID;
        string user2ID = MatchSceneManager.User2ID;

        var updates = new Dictionary<string, object>
        {
            { $"{user1ID}/inMatch", false },
            { $"{user2ID}/inMatch", false }
        };

        databaseRef.UpdateChildrenAsync(updates).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SceneManager.LoadScene("Game"); 
                });
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to update match exit status: " + task.Exception);
            }
        });
    }
}
