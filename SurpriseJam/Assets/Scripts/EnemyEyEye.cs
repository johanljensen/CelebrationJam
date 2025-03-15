using System.Collections;
using UnityEngine;

public class EnemyEyEye : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _body;

    [SerializeField] Transform _player;
    [SerializeField] float _speed;
    [SerializeField] float _health;
    [SerializeField] float _damage;
    [SerializeField] float timeBetweenAttacks;

    float timeSinceLastAttack = 0;
    bool touchingPlayer = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player)
        {
            Vector3 dir = _player.position - transform.position;
            Vector3 desiredPosition = transform.position + dir.normalized * Time.fixedDeltaTime * _speed;

            _rb.linearVelocity = dir.normalized  * _speed;

            //Math to get angle
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            _body.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        if (touchingPlayer)
        {
            timeSinceLastAttack += Time.fixedDeltaTime;

            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                timeSinceLastAttack = 0;
                _player.GetComponent<PlayerController>().TakeDamage(_damage);
            }
        }
    }

    public bool TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            StartCoroutine(DieNextFrame());
            return true;
        }
        return false;
    }

    IEnumerator DieNextFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            timeSinceLastAttack = timeBetweenAttacks;
            touchingPlayer = true;
        }
    }private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
}
