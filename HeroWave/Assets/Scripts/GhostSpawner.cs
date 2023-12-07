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

        // Запускаем корутину для спауна призраков
        StartCoroutine(SpawnGhosts());
    }

    private IEnumerator SpawnGhosts()
    {
        while (true)
        {
            // Создаем новый призрак
            SpawnGhost();

            // Ждем указанный интервал времени перед следующим спауном
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnGhost()
    {
        // Генерируем случайное число для определения стороны спауна (лево или право)
        float randomSide = Random.Range(0f, 1f);
        float spawnX;

        // Определяем координату X в зависимости от случайного числа
        if (randomSide < 0.5f)
        {
            // Спаун слева от игрока
            spawnX = player.transform.position.x - Random.Range(5f, 10f);
        }
        else
        {
            // Спаун справа от игрока
            spawnX = player.transform.position.x + Random.Range(5f, 10f);
        }

        // Создаем позицию для спауна
        Vector3 spawnPosition = new Vector3(spawnX, player.transform.position.y, player.transform.position.z);

        // Создаем новый призрак на указанной позиции
        Instantiate(ghostPrefab, spawnPosition, Quaternion.identity);
    }
}
