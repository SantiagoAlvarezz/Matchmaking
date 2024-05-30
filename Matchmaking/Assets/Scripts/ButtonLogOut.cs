using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLogOut : MonoBehaviour
{
    public void HandleLogoutButtonClicked()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var userId = auth.CurrentUser.UserId;
        auth.SignOut();

        var userStatus = new Dictionary<string, object>
    {
        { "online", false }
    };
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).UpdateChildrenAsync(userStatus);
    }

}
