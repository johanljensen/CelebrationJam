using UnityEngine;

public class EnemySpammer : MonoBehaviour
{
    [SerializeField] float _timeBeforeSpawn = 5;
    float _timer = 0;

    [SerializeField] GameObject _enemyPrefab;
    Plane _plane = new Plane(Vector3.up, Vector3.zero);

    [SerializeField] Transform player;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeBeforeSpawn)
        {
            _timer = 0;
            SpawnEnemy();
        }
    }

    Vector3 GenerateSpawnPosition()
    {

        int chosenAxis = Random.Range(0, 2);
        float xPos;
        float yPos;
        int chosenSide = Random.Range(0, 2);

        if (chosenAxis == 0)
        {
            yPos = Random.Range(-0.1f, 1.1f);
            xPos = chosenSide == 0 ? -0.1f : 1.1f; // 0 - Left side, 1 - Right side
        }
        else
        {
            xPos = Random.Range(-0.1f, 1.1f);
            yPos = chosenSide == 0 ? -0.1f : 1.1f; // 0 - Left side, 1 - Right side
        }

        return new Vector3(xPos,yPos, 10f);
    }

    void SpawnEnemy()
    {
        var instEnemy = Instantiate(_enemyPrefab);
        instEnemy.GetComponent<EnemyEyEye>().SetPlayer(player);

        Ray ray = Camera.main.ViewportPointToRay(GenerateSpawnPosition());
        float distance = 0.0f;
        if (_plane.Raycast(ray, out distance))
        {

            //Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(distance);

            instEnemy.transform.position = hitPoint;
        }
    }

}
