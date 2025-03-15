using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private float _maximumHealth = 100f;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _shotLength;
    [SerializeField] private float _hitForce = 100f;

    Plane _plane = new Plane(Vector3.up, Vector3.zero);
    [SerializeField] private GameObject _body;
    [SerializeField] private LineRenderer _shootLine;
    [SerializeField] private Transform _gunEnd;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private float shotDuration = 0.07f;
    
    [SerializeField] private Healthbar _healthbar;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Initialize health bar
        if (_healthbar != null)
        {
            _healthbar.SetHealth(_currentHealth, _maximumHealth);
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _rb.linearVelocity = dir.normalized * _speed;  // Fixed mistake: _rb.linearVelocity → _rb.velocity
        RotateBody();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void RotateBody()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 0.0f;
        if (_plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);

            Vector3 direction = new Vector3(
                hitPoint.x - transform.position.x,
                hitPoint.y - transform.position.y,
                hitPoint.z - transform.position.z
            );

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            _body.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    void Shoot()
    {
        _shootLine.SetPosition(0, _gunEnd.position);

        Ray shootRay = new Ray(_body.transform.position, _body.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(shootRay, out hit, _shotLength))
        {
            _shootLine.SetPosition(1, hit.point);

            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.rigidbody.AddForce(-hit.normal * _hitForce);
                bool enemyDead = hit.transform.GetComponent<EnemyEyEye>().TakeDamage(_damage);

                if (enemyDead)
                {
                    if (_levelHandler)
                        _levelHandler.EnemyDied();
                }
            }
        }
        else
        {
            _shootLine.SetPosition(1, _body.transform.position + _body.transform.forward * _shotLength);
        }

        StartCoroutine(ShotEffect());
    }

    private IEnumerator ShotEffect()
    {
        _shootLine.enabled = true;
        yield return new WaitForSeconds(shotDuration);
        _shootLine.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maximumHealth); // Prevent negative health

        // Update health bar
        if (_healthbar != null)
        {
            _healthbar.SetHealth(_currentHealth, _maximumHealth);
        }

        if (_currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }


    private void OnMouseDown() // "test" click on the priest and he takes Damage
    {
        _currentHealth -= 10;
        _healthbar.SetHealth(_currentHealth, _maximumHealth);
    }
}

// orginal code for PlayerController
/*
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float health;
    [SerializeField] float _speed;
    [SerializeField] float _damage;
    [SerializeField] float _shotLength;
    [SerializeField] float _hitForce = 100f;

    Plane _plane = new Plane(Vector3.up, Vector3.zero);
    [SerializeField] GameObject _body;
    [SerializeField] LineRenderer _shootLine;
    [SerializeField] Transform _gunEnd;
    [SerializeField] LevelHandler _levelHandler;

    [SerializeField] float shotDuration = 0.07f;
    
    [SerializeField] private Healthbar _healthbar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));

        _rb.linearVelocity = dir.normalized * _speed;
        RotateBody();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void RotateBody()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 0.0f;
        if (_plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            //Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

            // create a direction vector (hitPoint => somePoint
            Vector3 direction = new Vector3(
                hitPoint.x - transform.position.x,
                hitPoint.y - transform.position.y,
                hitPoint.z - transform.position.z
                );

            //Math to get angle
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            _body.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    void Shoot()
    {
        _shootLine.SetPosition(0, _gunEnd.position);

        Ray shootRay = new Ray(_body.transform.position, _body.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(shootRay, out hit, _shotLength))
        {
            _shootLine.SetPosition(1, hit.point);

            if (hit.transform.gameObject.tag == "Enemy")
            {
                hit.rigidbody.AddForce(- hit.normal * _hitForce);
                bool enemyDead = hit.transform.GetComponent<EnemyEyEye>().TakeDamage(_damage);

                if (enemyDead)
                {
                    if (_levelHandler)
                        _levelHandler.EnemyDied();
                }
            }
        }
        else
        {
            _shootLine.SetPosition(1, _body.transform.position + _body.transform.forward * _shotLength);
        }

        StartCoroutine(ShotEffect());
    }

    private IEnumerator ShotEffect()
    {
        // Turn on our line renderer
        _shootLine.enabled = true;

        //Wait for .07 seconds
        yield return new WaitForSeconds(shotDuration);

        // Deactivate our line renderer after waiting
        _shootLine.enabled = false;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
*/