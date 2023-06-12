using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;    // Prefab là đối tượng đc khởi tạo nhiều lần (Share referrences)
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerup;

    private bool _stopSpawning = false;

    public void StartSpawming()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        //StartCoroutine("SpawnRoutine");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //spawn đối tượng enemy theo 1 khoảng thgian nhất định
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.0f);
        }
    }

    //spawn ngẫu nhiên 1 trong 3 loại power up
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            //GameObject newPowerup = Instantiate(_tripleShotPrefab, posToSpawn, Quaternion.identity);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerup[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 5));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
