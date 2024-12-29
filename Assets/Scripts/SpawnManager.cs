using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powrups;
    
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        GameObject newEnemy;
        Vector3 posToSPawn;
        while (_stopSpawning == false) {
            posToSPawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            newEnemy = Instantiate(_enemyPrefab, posToSPawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    //never get here!!!
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        //every 3-7 sec spawn in a power up
        while (_stopSpawning == false) {
            Vector3 posToSPawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            //Instantiate(_tripleShotPowerupPrefab, posToSPawn, Quaternion.identity);
            Instantiate(powrups[Random.Range(0,3)], posToSPawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath() {
        Debug.Log("SpawnManager: Player died, stopping enemy spawn.");
        _stopSpawning = true;
    }
}
