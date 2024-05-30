using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogOut : MonoBehaviour
{
    public void HandleLogoutButtonClicked()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var userId = auth.CurrentUser.UserId;
        auth.SignOut();

        var userStatus = new Dictionary<string, object>
        {
            { "online", false },
            { "inMatch", false }
        };
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).UpdateChildrenAsync(userStatus).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SceneManager.LoadScene("Login"); 
                });
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Failed to update user status: " + task.Exception);
            }
        });
    }
}
