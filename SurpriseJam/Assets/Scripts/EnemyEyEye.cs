using UnityEngine;

public class EnemyEyEye : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed;
    [SerializeField] GameObject _body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

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
    }
}
