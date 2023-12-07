using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private float spawnInterval = 5f;

    private Camera mainCamera;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player Tag is not found");
        }

        mainCamera = Camera.main;

        StartCoroutine(SpawnMobs());
    }

    private IEnumerator SpawnMobs()
    {
        while (true)
        {
            SpawnMob();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMob()
    {
        float screenWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;

        float randomSide = Random.Range(0f, 1f);
        float spawnX;

        if (randomSide < 0.5f)
        {
            spawnX = player.transform.position.x - screenWidth * 0.5f - Random.Range(5f, 10f);
        }
        else
        {
            spawnX = player.transform.position.x + screenWidth * 0.5f + Random.Range(5f, 10f);
        }

        Vector3 spawnPosition = new Vector3(spawnX, player.transform.position.y, player.transform.position.z);

        Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
    }
}
