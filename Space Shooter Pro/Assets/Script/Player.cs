using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speed = 3.5f, _speedMultiplier = 3f;
    [SerializeField]
    private float _fireRate = 0.5f, _canFire = -1f;
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _shieldVisualizerPrefab, _thrusterVisualizerPrefab;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine, _explosionPrefab;
    [SerializeField]
    private bool _isTripleShotActive = false, _isSpeedActive = false, _isShieldActive = false;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;
    private UIManager _uIManager;
    private SpawnManager _spawnManager;
    private InputProvider _inputProvider;

    private void OnEnable()
    {
        _inputProvider = new InputProvider();
        _inputProvider.shootPerform += FireLaser;
        _inputProvider.Enable();
    }


    private void OnDisable()
    {
        _inputProvider.shootPerform -= FireLaser;
        _inputProvider.Disable();
    }

    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        #region Check Nulll
        if (_uIManager == null)
        {
            Debug.LogError("The UI Manager Is Null!");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager Is Null!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source On Player Is Null!");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
        #endregion

        _thrusterVisualizerPrefab.SetActive(true);
        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);
    }

    void Update()
    {
        calculateMovement();
        /*if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }*/

    }

    #region Game Play Calculation

    void calculateMovement()
    {
        /*        float horizontalInput = Input.GetAxis("Horizontal");  
                float verticalInput = Input.GetAxis("Vertical");*/

        Vector2 input = _inputProvider.MovementInput();
        Vector3 direction = new Vector3(input.x, input.y, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        /*
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)     
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }*/

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 3.8f), 0);  //code xử lý boundary y với Mathf.Clamp

        //code xử lý boundary x 
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

    }

    void FireLaser(InputAction.CallbackContext context)
    {
        //Cooldown
        if (Time.time > _canFire)
        {
            //FireRate
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            _audioSource.Play();
        }
    }

    public void Damaged()
    {
        if (_isShieldActive)
        {
            ShieldDeactive();
            return;
        }


        _lives--;
        _uIManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives == 0)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.OnPlayerDeath();
            _uIManager.CheckForBestScore();
            Destroy(this.gameObject);
        }
    }

    public void AddScore()
    {
        _uIManager.UpdateScore();
    }

    #endregion

    #region Power up Visualizer

    public void ShieldActive()
    {
        _shieldVisualizerPrefab.SetActive(true);
        _isShieldActive = true;
    }

    public void ShieldDeactive()
    {
        _isShieldActive = false;
        _shieldVisualizerPrefab.SetActive(false);
    }

    #endregion

    #region Power up Routine

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedActive()
    {
        _isSpeedActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
        _speed /= _speedMultiplier;
    }

    #endregion

}
