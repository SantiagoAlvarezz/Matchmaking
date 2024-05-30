using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;

public class DisplayUsers : MonoBehaviour
{
    [SerializeField] private Transform userListContainer;  
    [SerializeField] private GameObject userItemPrefab;   

    private DatabaseReference databaseRef;

    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.GetReference("users");
        LoadUsers();
    }

    private void LoadUsers()
    {
        databaseRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve users: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    foreach (Transform child in userListContainer)
                    {
                        Destroy(child.gameObject);
                    }

                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        string username = userSnapshot.Child("username").Value.ToString();
                        bool isOnline = bool.Parse(userSnapshot.Child("online").Value.ToString());

                        GameObject userItem = Instantiate(userItemPrefab, userListContainer);
                        userItem.GetComponentInChildren<TMP_Text>().text = username;
                        userItem.GetComponent<Image>().color = isOnline ? Color.green : Color.red; // Green for online, red for offline
                    }
                });
            }
        });
    }
}
