using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using LootLocker.Requests;
using UnityEngine;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private LeaderboardLine linePrefab;
    [SerializeField] private Transform linesContainer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private LeaderboardLine playerLine;

    private List<LeaderboardLine> _lines = new();

    public void Fetch(int leaderboardId)
    {
        canvasGroup.alpha = 0;
        DestroyLines();
        LootLockerSDKManager.GetScoreList(leaderboardId, 10, OnGetScoreResponse);
    }

    public void InitPlayerLine(string name, int rank, int score)
    {
        playerLine.Init(name, rank, score);
    }

    private void DestroyLines()
    {
        foreach (var line in _lines)
        {
            Destroy(line.gameObject);
        }
        _lines.Clear();
    }

    private void OnGetScoreResponse(LootLockerGetScoreListResponse response)
    {
        if (response.statusCode == 200) {
            Debug.Log("Successful");
            InitLines(response.items);
            canvasGroup.DOFade(1, 0.5f);
        } else {
            Debug.Log("failed: " + response.Error);
        }
    }

    private void InitLines(LootLockerLeaderboardMember[] members)
    {
        foreach (var member in members)
        {
            var line = Instantiate(linePrefab, linesContainer);
            line.name = $"Line {member.rank}";
            line.Init(member);
            _lines.Add(line);
        }
    }
}