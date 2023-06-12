using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;        // tốc độ di chuyển của Enemt

    private Player _player;

    private Animator _animator;

    AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _animator = GetComponent<Animator>();

        _audioSource = GetComponent<AudioSource>();

        if(_animator == null)
        {
            Debug.LogError("The animator is null!");
        }

        if(_player == null)
        {
            Debug.LogError("The player is null!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Hit: " + other.name);
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damaged();
            }

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //nên tạo 1 biến private trong class này để lưu vào cache (handle)
            //thay vì gọi thường xuyên như cách dưới
            //Player player = GameObject.Find("Player").GetComponent<Player>();
            if (_player != null)
            {
               _player.AddScore();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

    }
}
