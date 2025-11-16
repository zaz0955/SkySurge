using UnityEngine;

public enum Rarity { Common, Rare, Epic }

[CreateAssetMenu(menuName = "SkySurge/UpgradeCard")]
public class UpgradeCard : ScriptableObject
{
    public string id;
    public string displayName;
    [TextArea] public string description;

    public Rarity rarity;

    // àÍ¿à¿¡µì
    public int addMaxHp;
    public int addDamage;
    public float addMoveSpeed;
    public float fireRateMultiplier = 1f;  // <1 = ÂÔ§àÃçÇ¢Öé¹
}
