using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour, IHitTarget
{
    [Header("Stats")]
    public int hp = 100;
    public int maxHp = 100;
    public int lv = 1;
    public int exp = 0;
    public float moveSpeed = 6f;

    [Header("Refs")]
    public Weapon weapon;
    Rigidbody2D rb;

    Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.I.state != GameState.Playing) return;

        // เคลื่อนที่
        rb.linearVelocity = moveInput * moveSpeed;

        // ยิงเมื่อกดคลิกซ้าย
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShootAtMouse();
        }
    }

    void ShootAtMouse()
    {
        // 1) ตำแหน่งเมาส์บนโลก (world)
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // 2) ทิศทางจาก player → เมาส์
        Vector2 dir = (mousePos - (Vector2)transform.position).normalized;

        // 3) ยิง
        weapon.Fire(transform.position, dir);
    }

    // New Input System event (PlayerInput)
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.I.state != GameState.Playing) return;
        hp -= amount;
        GameManager.I.ui.UpdateHUD(hp, lv, exp, GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
        if (hp <= 0) Die();
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(maxHp, hp + amount);
        GameManager.I.ui.UpdateHUD(hp, lv, exp, GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
    }

    void Die()
    {
        rb.linearVelocity = Vector2.zero;
        weapon.enabled = false;
        GameManager.I.End();
        gameObject.SetActive(false);
    }
    int ExpToNextLevel()
    {
        return 5 + (lv - 1) * 5;
    }
    public void LevelUp()
    {
        lv++;
        GameManager.I.OnPlayerLevelUp();
    }
    public void AddExp(int value)
    {
        exp += value;
        while (exp >= ExpToNextLevel())
        {
            exp -= ExpToNextLevel();
            LevelUp();
        }
        GameManager.I.ui.UpdateHUD(hp, lv, exp, GameManager.I.waveMgr.WaveIndex, GameManager.I.score);
    }
}
