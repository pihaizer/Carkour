using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class LeaderboardLine : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text scoreText;

    public void Init(LootLockerLeaderboardMember member) => 
        Init(member.player.name, member.rank, member.score);

    public void Init(string name, int rank, int score)
    {
        nameText.text = string.IsNullOrEmpty(name) ? "Anonymous" : name;
        rankText.text = rank.ToString();
        scoreText.text = (score / 1e5f).ToString("F2");
    }
}