using System.Collections;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float spawnInterval = 5f;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the tag 'Player'.");
        }

        // ��������� �������� ��� ������ ���������
        StartCoroutine(SpawnGhosts());
    }

    private IEnumerator SpawnGhosts()
    {
        while (true)
        {
            // ������� ����� �������
            SpawnGhost();

            // ���� ��������� �������� ������� ����� ��������� �������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnGhost()
    {
        // ���������� ��������� ����� ��� ����������� ������� ������ (���� ��� �����)
        float randomSide = Random.Range(0f, 1f);
        float spawnX;

        // ���������� ���������� X � ����������� �� ���������� �����
        if (randomSide < 0.5f)
        {
            // ����� ����� �� ������
            spawnX = player.transform.position.x - Random.Range(5f, 10f);
        }
        else
        {
            // ����� ������ �� ������
            spawnX = player.transform.position.x + Random.Range(5f, 10f);
        }

        // ������� ������� ��� ������
        Vector3 spawnPosition = new Vector3(spawnX, player.transform.position.y, player.transform.position.z);

        // ������� ����� ������� �� ��������� �������
        Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
    }
}
