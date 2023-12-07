using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI gameOverKillCountText;


    private int currentAttack = 1;
    private int hurtCount = 0;
    private int remainingLives = 3;
    private int killCount = 0;
    private bool isDead = false;

    private void Start()
    {
        animator = GetComponent <Animator>();

        if (livesText == null || killCountText == null || gameOverMenu == null)
        {
            Debug.LogError("One or more components not assigned in the Inspector");
        }
        else
        {
            UpdateLivesDisplay();
            UpdateKillCountDisplay();
        }
    }
    private void Update()
    {
        if (!isDead)
        {
            PlayerMovementInput();
            PlayerAttackInput();
        }
    }

    private void UpdateLivesDisplay()
    {
        livesText.text = $"Lives: {remainingLives}";
    }

    private void UpdateKillCountDisplay()
    {
        killCountText.text = $"Kills: {killCount}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && collision.tag == "Mob")
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                float distanceToMob = Vector2.Distance(transform.position, collision.transform.position);

                if (distanceToMob <= attackRadius)
                {
                    animator.SetTrigger("Hurt");

                    remainingLives--;
                    UpdateLivesDisplay();

                    if (remainingLives <= 0)
                    {
                        StartCoroutine(DeathSequence());
                    }
                }
            }
        }
    }

    private void PlayerMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0f)
        {
            RotateCharacter(horizontalInput);
        }
    }

    private void RotateCharacter(float horizontalInput)
    {
        float targetRotation = Mathf.Atan2(0, horizontalInput) * Mathf.Rad2Deg;
        Vector3 newRotation = new Vector3(0, targetRotation, 0);
        transform.rotation = Quaternion.Euler(newRotation);
    }

    private void PlayerAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PerformAttack();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        currentAttack = (currentAttack % 3) + 1;
        string attackTrigger = "Attack" + currentAttack;
        animator.SetTrigger(attackTrigger);

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 attackDirection = new Vector2(horizontalInput, 0f).normalized;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Mob"))
            {
                MobController mob = collider.GetComponent<MobController>();
                if (mob != null)
                {
                    Vector2 mobDirection = (mob.transform.position - transform.position).normalized;
                    float angle = Vector2.SignedAngle(attackDirection, mobDirection);

                    if (Mathf.Abs(angle) < 45f)
                    {
                        mob.TakeDamage();
                    }
                }
            }
        }
    }

    public void OnHurtAnimationEnd()
    {
        hurtCount++;

        if (hurtCount >= 3)
        {
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        animator.SetTrigger("Death");
        isDead = true;

        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;

        if (gameOverMenu != null)
        {
            if (gameOverKillCountText != null)
            {
                gameOverKillCountText.text = $"Total Kills: {killCount}";
            }

            gameOverMenu.SetActive(true);
        }
    }

    public void UpdateKillCount()
    {
        killCount++;
        UpdateKillCountDisplay();
    }
}
