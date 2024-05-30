using UnityEngine;
using TMPro;

public class MatchSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text user1Text;
    [SerializeField] private TMP_Text user2Text;

    private static string user1Name;
    private static string user2Name;

    public static void SetMatchUsers(string user1, string user2)
    {
        user1Name = user1;
        user2Name = user2;
    }

    void Start()
    {
        user1Text.text = user1Name;
        user2Text.text = user2Name;
    }
}
