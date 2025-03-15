using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] float _speed;

    Plane _plane = new Plane(Vector3.up, Vector3.zero);
    [SerializeField] GameObject _body;

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
    }

    void RotateBody()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 0.0f;
        if (_plane.Raycast(ray, out distance))
        {
            //Debug.Log("distance " + distance);

            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(distance);

            //Draw a debug ray to see where you are hitting
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
}
