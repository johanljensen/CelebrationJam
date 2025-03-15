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

    [SerializeField] float shotDuration = 0.07f;

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
                hit.transform.GetComponent<EnemyEyEye>().TakeDamage(_damage);
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
