using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Refs")]
    public Slider hpBar;
    public Slider expBar;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;

    public void UpdateHUD(int hp, int lv, int exp, int wave, int score)
    {
        hpBar.value = hpBar.maxValue > 0 ? hp : hpBar.value;
        expBar.value = exp; // ไว้ค่อยแม็ปกับ nextLevelExp จริง ๆ
        waveText.text = $"Wave : {wave}";
        scoreText.text = $"Score : {score}";
    }

    public void ShowGameOver(int score)
    {
        gameOverPanel.SetActive(true);
        scoreText.text = $"Score: {score}";
    }

    [Header("Upgrade UI")]
    public GameObject upgradePanel;
    public Button[] cardButtons;
    public TextMeshProUGUI[] cardTitleTexts;
    public TextMeshProUGUI[] cardDescTexts;

    Action<UpgradeCard> onPickCallback;
    List<UpgradeCard> currentChoices;

    public void ShowUpgrade(List<UpgradeCard> choices, Action<UpgradeCard> onPick)
    {
        upgradePanel.SetActive(true);
        currentChoices = choices;
        onPickCallback = onPick;

        for (int i = 0; i < cardButtons.Length; i++)
        {
            if (i < choices.Count)
            {
                var card = choices[i];
                cardButtons[i].gameObject.SetActive(true);
                cardTitleTexts[i].text = card.displayName;
                cardDescTexts[i].text = card.description;

                int idx = i; // ป้องกันปัญหา closure
                cardButtons[i].onClick.RemoveAllListeners();
                cardButtons[i].onClick.AddListener(() => OnCardClicked(idx));
            }
            else
            {
                cardButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideUpgrade()
    {
        upgradePanel.SetActive(false);
    }

    void OnCardClicked(int index)
    {
        if (currentChoices == null || index >= currentChoices.Count) return;

        var card = currentChoices[index];
        onPickCallback?.Invoke(card);
    }
}


