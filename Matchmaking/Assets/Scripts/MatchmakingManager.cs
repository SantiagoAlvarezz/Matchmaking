using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchmakingManager : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText; 
    private DatabaseReference databaseRef;
    private FirebaseAuth auth;

    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.GetReference("users");
        auth = FirebaseAuth.DefaultInstance;
    }

    public void HandleFindMatchButtonClicked()
    {
        var currentUserId = auth.CurrentUser.UserId;

        // Retrieve online users who are not in a match
        databaseRef.OrderByChild("online").EqualTo(true).GetValueAsync().ContinueWith(task2 =>
        {
            if (task2.IsFaulted)
            {
                Debug.LogError("Failed to retrieve users: " + task2.Exception);
                return;
            }

            DataSnapshot snapshot = task2.Result;
            List<DataSnapshot> availableUsers = new List<DataSnapshot>();

            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                if (userSnapshot.Key != currentUserId &&
                    userSnapshot.Child("inMatch").Value != null &&
                    !(bool)userSnapshot.Child("inMatch").Value)
                {
                    availableUsers.Add(userSnapshot);
                }
            }

            if (availableUsers.Count > 0)
            {
                DataSnapshot matchUserSnapshot = availableUsers[0]; // Pick the first available user
                string matchUserId = matchUserSnapshot.Key;
                string currentUserName = snapshot.Child(currentUserId).Child("username").Value.ToString();
                string matchUserName = matchUserSnapshot.Child("username").Value.ToString();

                var updates = new Dictionary<string, object>
                {
                    { $"{currentUserId}/inMatch", true },
                    { $"{matchUserId}/inMatch", true }
                };

                databaseRef.UpdateChildrenAsync(updates).ContinueWith(task3 =>
                {
                    if (task3.IsCompleted)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            // Pass the matched usernames and ids to the match scene
                            MatchSceneManager.SetMatchUsers(currentUserName, matchUserName, currentUserId, matchUserId);
                            SceneManager.LoadScene("Match"); 
                        });
                    }
                    else if (task3.IsFaulted)
                    {
                        Debug.LogError("Failed to update match status: " + task3.Exception);
                    }
                });
            }
            else
            {
                DisplayNoUsersMessage();
            }
        });
    }

    private void DisplayNoUsersMessage()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            statusText.text = "There are no online users, please try again later.";
        });
    }
}
