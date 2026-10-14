using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    [Header("New Record Flash Settings")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color recordFlashColor = Color.yellow;
    [SerializeField] private float flashDuration = 0.5f;

    private int currentScore = 0;
    private int bestScore = 0;

    private void OnEnable()
    {
        // Subscribe to the central event hub
        GameEventManager.OnItemCaught += HandleItemCaught;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        GameEventManager.OnItemCaught -= HandleItemCaught;
    }

    private void Start()
    {
        // Load persistent high score from previous sessions
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateUI();
    }

    private void HandleItemCaught(FallingObject item)
    {
        // Only react if it's a Good item (Seal)
        if (item.itemType == FallingObject.ObjectType.Good)
        {
            currentScore++;
            CheckForNewRecord();
            UpdateUI();
        }
    }

    private void CheckForNewRecord()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();

            // Trigger the new record visual alert
            StartCoroutine(FlashNewRecordEffect());
        }
    }

   private void UpdateUI()
{
    if (scoreText != null) scoreText.text = "Score: " + currentScore;
    if (bestScoreText != null) bestScoreText.text = "Best Record: " + bestScore;
}
    private IEnumerator FlashNewRecordEffect()
    {
        if (bestScoreText != null)
        {
            bestScoreText.color = recordFlashColor;
            bestScoreText.transform.localScale = Vector3.one * 1.2f;

            yield return new WaitForSeconds(flashDuration);

            bestScoreText.color = normalColor;
            bestScoreText.transform.localScale = Vector3.one;
        }
    }
}