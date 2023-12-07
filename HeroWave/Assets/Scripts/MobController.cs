using System.Collections;
using UnityEngine;

public class MobController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Animator animator;

    private GameObject player;
    private bool canHarmPlayer = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the tag 'Player'.");
        }
    }

    private void Update()
    {
        if (player != null && canHarmPlayer)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

    public void TakeDamage()
    {
        if (animator != null)
        {
            canHarmPlayer = false;
            animator.SetTrigger("Death");
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        GameObject playerController = GameObject.FindGameObjectWithTag("Player");
        if (playerController != null)
        {
            PlayerController playerControllerScript = playerController.GetComponent<PlayerController>();
            if (playerControllerScript != null)
            {
                playerControllerScript.UpdateKillCount();
            }
        }

        Destroy(gameObject);
    }
}
