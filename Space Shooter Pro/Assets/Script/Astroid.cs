using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionPrefab;

    private float _rotateSpeed = 19.0f;

    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager Is NULL!");
        }


    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Laser")
        {
            _spawnManager.StartSpawming();
            Destroy(this.gameObject, 0.2f);
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damaged();
            }
            _spawnManager.StartSpawming();
            Destroy(this.gameObject, 0.2f);
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
        }
    }
}
