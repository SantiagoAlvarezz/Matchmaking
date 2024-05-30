using UnityEngine;
using TMPro;

public class MatchSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text user1Text;
    [SerializeField] private TMP_Text user2Text;

    private static string user1Name;
    private static string user2Name;
    public static string User1ID;
    public static string User2ID;

    public static void SetMatchUsers(string user1, string user2, string id1, string id2)
    {
        user1Name = user1;
        user2Name = user2;
        User1ID = id1;
        User2ID = id2;
    }

    void Start()
    {
        user1Text.text = user1Name;
        user2Text.text = user2Name;
    }
}
