using UnityEngine;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] List<UpgradeCard> pool = new();
    List<UpgradeCard> currentChoices = new();

    public List<UpgradeCard> RollChoices(int playerLv, int waveIdx)
    {
        currentChoices.Clear();

        int count = Mathf.Min(3, pool.Count);
        for (int i = 0; i < count; i++)
        {
            // สุ่มแบบง่าย (ไม่กันซ้ำขั้นสูง แต่ใช้ while กันซ้ำเบื้องต้น)
            UpgradeCard card;
            do
            {
                card = pool[Random.Range(0, pool.Count)];
            }
            while (currentChoices.Contains(card) && currentChoices.Count < pool.Count);

            currentChoices.Add(card);
        }

        return currentChoices;
    }

    public void ApplyUpgrade(Player player, UpgradeCard card)
    {
        player.maxHp += card.addMaxHp;
        player.hp = Mathf.Min(player.hp + card.addMaxHp, player.maxHp);
        player.moveSpeed += card.addMoveSpeed;

        if (player.weapon != null)
        {
            player.weapon.damageBonus += card.addDamage;
            player.weapon.fireRate *= card.fireRateMultiplier;
        }

        // อัปเดต HUD อีกที
        GameManager.I.ui.UpdateHUD(player.hp, player.lv, player.exp,
                                   GameManager.I.waveMgr.WaveIndex,
                                   GameManager.I.score);
    }
}
