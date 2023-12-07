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
            Debug.LogError("One or more components not assigned in the Inspector.");
        }
        else
        {
            UpdateLivesDisplay();
            UpdateKillCountDisplay();
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

    private void Update()
    {
        if (!isDead)
        {
            PlayerMovementInput();
            PlayerAttackInput();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isDead && collision.tag == "Ghost")
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                float distanceToGhost = Vector2.Distance(transform.position, collision.transform.position);

                if (distanceToGhost <= attackRadius)
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
            if (collider.CompareTag("Ghost"))
            {
                GhostController ghost = collider.GetComponent<GhostController>();
                if (ghost != null)
                {
                    Vector2 ghostDirection = (ghost.transform.position - transform.position).normalized;
                    if (Vector2.Dot(ghostDirection, attackDirection) > 0.5f)
                    {
                        ghost.TakeDamage();
                    }
                }
            }
        }

        Debug.Log("Player performed " + attackTrigger + "!");
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

        yield return new WaitForSeconds(2f); // Delay for 2 seconds

        // Pause the game
        Time.timeScale = 0f;

        // Show the game over menu
        if (gameOverMenu != null)
        {
            // Set the kill count text on the game over panel
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
