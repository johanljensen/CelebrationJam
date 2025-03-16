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
    [SerializeField] private float _timeBetweenShots = 0.25f;
    private float _shotTimer = 0;
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private int _ammoAmount = 8;
    int _shots = 0;
    bool _reloading = false;

    Plane _plane = new Plane(Vector3.up, Vector3.zero);
    [SerializeField] private GameObject _body;
    [SerializeField] private LineRenderer _shootLine;
    [SerializeField] private Transform _gunEnd;
    [SerializeField] private float _shotDuration = 0.07f;
    
    [SerializeField] private Healthbar _healthbar; 
    [SerializeField] private RectTransform _levelbar;
    [SerializeField] private Animator _animator;
    float _speedAdjustment = 1;

    [SerializeField] GameObject deadUI;
    bool _dead = false;

    // No need to serialize this since we're using the singleton pattern
    private ScoreManager _scoreManager;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        // Get reference to ScoreManager
        _scoreManager = ScoreManager.Instance;

        // Initialize health bar
        if (_healthbar != null)
        {
            _healthbar.SetHealth(_currentHealth, _maximumHealth);
        }
    }

    void FixedUpdate()
    {
        if (_dead)
            return;
        Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        _rb.linearVelocity = dir.normalized * _speed * _speedAdjustment;
        RotateBody(dir);

        if (_reloading)
        {
            _shotTimer += Time.fixedDeltaTime;
            float t = _shotTimer / _reloadTime;
            _levelbar.localScale = new Vector3(t, 1f, 1f);

            if (_shotTimer >= _reloadTime)
            {
                _levelbar.gameObject.SetActive(false);
                _reloading = false;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (_reloading)
                return;

            _shotTimer += Time.fixedDeltaTime;
            if (_shotTimer >= _timeBetweenShots)
            {
                Shoot();
                _shots++;
                _shotTimer = 0;

                if (_shots > _ammoAmount)
                {
                    _shots = 0;
                    _reloading = true;
                    _levelbar.gameObject.SetActive(true);
                    _levelbar.localScale = new Vector3(0, 1f, 1f);
                }
            }
        }
    }

    void RotateBody(Vector3 playerDir)
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

            feetDirection = (_body.transform.forward - playerDir).normalized;
            float feetAngle = Mathf.Atan2(feetDirection.x, feetDirection.z) * Mathf.Rad2Deg;
            _animator.SetFloat("dir",(feetAngle)/360f);
            if (playerDir == Vector3.zero)
            {
                _animator.SetBool("Idle",true);
            }
            else
            {
                _animator.SetBool("Idle", false);
            }
        }
    }
    Vector3 feetDirection;

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
                    // Register kill with ScoreManager
                    if (_scoreManager != null)
                        _scoreManager.AddKill();
                    
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
        yield return new WaitForSeconds(_shotDuration);
        _shootLine.enabled = false;
    }
    public void ChangeSpeedAdjustment(float value)
    {
        _speedAdjustment = value;
    }

    public void Dead()
    {
        deadUI.SetActive(true);
    }

    public void TakeDamage(float damage)
    {
        if (_dead)
            return;
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maximumHealth); // Prevent negative health

        // Update health bar
        if (_healthbar != null)
        {
            _healthbar.SetHealth(_currentHealth, _maximumHealth);
        }

        if (_currentHealth <= 0)
        {
            //gameObject.SetActive(false);
            _dead = true;
            _animator.SetBool("dead", true);
        }
    }

    /*private void OnMouseDown() // "test" click on the priest and he takes Damage
    {
        _currentHealth -= 10;
        _healthbar.SetHealth(_currentHealth, _maximumHealth);
    }*/
}


/*
public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private float _maximumHealth = 100f;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _shotLength;
    [SerializeField] private float _hitForce = 100f;
    [SerializeField] private float _timeBetweenShots = 0.25f;
    private float _shotTimer = 0;
    [SerializeField] private float _reloadTime = 1f;
    [SerializeField] private int _ammoAmount = 8;
    int _shots = 0;
    bool _reloading = false;

    Plane _plane = new Plane(Vector3.up, Vector3.zero);
    [SerializeField] private GameObject _body;
    [SerializeField] private LineRenderer _shootLine;
    [SerializeField] private Transform _gunEnd;
    [SerializeField] private LevelHandler _levelHandler;
    [SerializeField] private float _shotDuration = 0.07f;
    
    [SerializeField] private Healthbar _healthbar; 
    [SerializeField] private RectTransform _levelbar;
    [SerializeField] private Animator _animator;
    float _speedAdjustment = 1;

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

        _rb.linearVelocity = dir.normalized * _speed * _speedAdjustment;  // Fixed mistake: _rb.linearVelocity → _rb.velocity
        RotateBody(dir);

        if (_reloading)
        {
            _shotTimer += Time.fixedDeltaTime;
            float t = _shotTimer / _reloadTime;
            _levelbar.localScale = new Vector3(t, 1f, 1f);

            if (_shotTimer >= _reloadTime)
            {
                _levelbar.gameObject.SetActive(false);
                _reloading = false;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (_reloading)
                return;

            _shotTimer += Time.fixedDeltaTime;
            if (_shotTimer >= _timeBetweenShots)
            {
                Shoot();
                _shots++;
                _shotTimer = 0;

                if (_shots > _ammoAmount)
                {
                    _shots = 0;
                    _reloading = true;
                    _levelbar.gameObject.SetActive(true);
                    _levelbar.localScale = new Vector3(0, 1f, 1f);
                }
            }
        }
    }

    void RotateBody(Vector3 playerDir)
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

            feetDirection = (_body.transform.forward - playerDir).normalized;
            float feetAngle = Mathf.Atan2(feetDirection.x, feetDirection.z) * Mathf.Rad2Deg;
            _animator.SetFloat("dir",(feetAngle)/360f);
            if (playerDir == Vector3.zero)
            {
                _animator.SetBool("Idle",true);
            }
            else
            {
                _animator.SetBool("Idle", false);
            }
        }
    }
    Vector3 feetDirection;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_body.transform.position, _body.transform.position +  feetDirection);
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
        yield return new WaitForSeconds(_shotDuration);
        _shootLine.enabled = false;
    }
    public void ChangeSpeedAdjustment(float value)
    {
        _speedAdjustment = value;
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
*/
