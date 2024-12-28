using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    // Határértékek konstansként definiálva.
    private const float HorizontalLimit = 11.3f;
    private const float VerticalUpperLimit = 0f;
    private const float VerticalLowerLimit = -3.8f;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;

    [SerializeField]
    private int _score;

    private UIManager _uIManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_spawnManager == null) {
            Debug.LogError("Spawn manager is null");
        }
    }

    void Update()
    {
        CalculateMovement();
        //hit space key spawn gameobject
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0).normalized;

        transform.Translate(direction * _speed * Time.deltaTime, Space.World);
   
        // Függőleges pozíció korlátozása.

        transform.position = new Vector3(
            transform.position.x, Mathf.Clamp(transform.position.y, VerticalLowerLimit, 0), 0);

        // Horizontális wrap-around (kijön az egyik oldalon, belép a másikon).
        transform.position = new Vector3(
         Mathf.Repeat(transform.position.x + HorizontalLimit, 2 * HorizontalLimit) - HorizontalLimit,
         transform.position.y, 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z);

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab,transform.position, Quaternion.identity);
        }
        else {
            Instantiate(_laserPrefab, spawnPosition, Quaternion.identity);
        }

    }

    public void Damage() {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        else { 
            _lives--;
            _uIManager.UpdateLives(_lives);
            //check if dead
            if (_lives < 1)
            {
                //communicate with spawn manager let know stop spawn
                _spawnManager.OnPlayerDeath();
                _uIManager.GameOverSequence();
                Destroy(this.gameObject);
            }
        }
    }

    public void TripleShotActive() { 
        _isTripleShotActive = true; 
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_isTripleShotActive)
        {
            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive= false;
        }
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    private IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_isSpeedBoostActive)
        {
            yield return new WaitForSeconds(5.0f);
            _isSpeedBoostActive = false;
            _speed /= speedMultiplier;

        }
    }

    public void ShieldActivate()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uIManager.UpdateScore(_score);
    }
}
