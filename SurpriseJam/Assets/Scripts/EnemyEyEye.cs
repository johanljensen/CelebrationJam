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
    [SerializeField] Animator _animator;
    [SerializeField] Collider _collider;
    [SerializeField] SkinnedMeshRenderer _meshRend;
    [SerializeField] Material _deathMat;
    bool _dead;
    [SerializeField] float _deathFadeTime = 1;
    float _deathFadeTimer = 0;
    bool fading;

    float _speedAdjustment = 1;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_dead)
        { 
            if(fading)
            {
                _deathFadeTimer += Time.fixedDeltaTime;
                float t = 1 - (_deathFadeTimer/ _deathFadeTime);
                Color col= new Color(1, 1, 1, t);
                _meshRend.material.color = col;

                if (t <= 0)
                    Destroy(gameObject);
            }
            return;
        }
        if (_player)
        {
            Vector3 dir = _player.position - transform.position;
            Vector3 desiredPosition = transform.position + dir.normalized * Time.fixedDeltaTime * _speed;

            _rb.linearVelocity = dir.normalized  * _speed  *_speedAdjustment;

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

    public void ChangeSpeedAdjustment(float value)
    {
        _speedAdjustment = value;
    }

    public bool TakeDamage(float damage)
    {

        _health -= damage;
        if (_health <= 0)
        {
            //StartCoroutine(DieNextFrame());
            _animator.SetBool("Death", true);
            _collider.enabled = false;
            Destroy(_rb);
            _dead = true;
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

    public void StartFading()
    {
        fading = true;

        _meshRend.material = _deathMat;
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
