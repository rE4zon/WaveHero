using System.Collections;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject mobPrefab;
    public float spawnInterval = 5f;

    private Camera mainCamera;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure the player has the tag 'Player'.");
        }

        mainCamera = Camera.main;

        // Запускаем корутину для спауна призраков
        StartCoroutine(SpawnMobs());
    }

    private IEnumerator SpawnMobs()
    {
        while (true)
        {
            // Создаем новый призрак
            SpawnMob();

            // Ждем указанный интервал времени перед следующим спауном
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMob()
    {
        // Получаем размеры экрана в мировых координатах
        float screenWidth = mainCamera.orthographicSize * 2 * mainCamera.aspect;

        // Генерируем случайное число для определения стороны спауна (лево или право)
        float randomSide = Random.Range(0f, 1f);
        float spawnX;

        // Определяем координату X в зависимости от случайного числа
        if (randomSide < 0.5f)
        {
            // Спаун слева от игрока, за границей экрана
            spawnX = player.transform.position.x - screenWidth * 0.5f - Random.Range(5f, 10f);
        }
        else
        {
            // Спаун справа от игрока, за границей экрана
            spawnX = player.transform.position.x + screenWidth * 0.5f + Random.Range(5f, 10f);
        }

        // Создаем позицию для спауна
        Vector3 spawnPosition = new Vector3(spawnX, player.transform.position.y, player.transform.position.z);

        // Создаем новый призрак на указанной позиции
        Instantiate(mobPrefab, spawnPosition, Quaternion.identity);
    }
}
