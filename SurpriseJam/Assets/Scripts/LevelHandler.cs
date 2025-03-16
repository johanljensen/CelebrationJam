using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class LevelHandler : MonoBehaviour
{
    float _experience = 0;
    int _level = 0;
    [SerializeField] float _xpToLevelUp = 3;

    [SerializeField] Vector2 _orthoSizeMinMax = new Vector2(7,20);
    float _curOrtoSize;
    float _nextOrtoSize;
    bool _changeOrthoSize = false;
    [SerializeField] float _orthoSizeChangeTime = 1;
    float _orthoSizeChangeTimer = 0;
    [SerializeField] int _maxOrthoLevel = 20;

    Camera _mainCam;
    [SerializeField] RectTransform _xpBar;

    [SerializeField] GameObject _giftPrefab;
    [SerializeField] Vector2 _giftSpawnRadiusMinMax = new Vector2(2, 10);


    Plane _plane = new Plane(Vector3.up, Vector3.zero);
    List<GameObject> gifts = new List<GameObject>();
    [SerializeField] EnemySpammer enemySpammer;
    [SerializeField] float spawnTimeDecrement; //Less than 1 please! and not 0 or under please

    [SerializeField] TMP_Text levelText;

    private void Start()
    {
        _mainCam = Camera.main;
        _mainCam.orthographicSize = _orthoSizeMinMax.x;
    }

    public void EnemyDied()
    {
        _experience += 1;

        float t = _experience / _xpToLevelUp;
        if (t >= 1)
            t = 0;

        _xpBar.localScale = new Vector3(t, 1f, 1f);
        if (_experience >= _xpToLevelUp)
        {
            _experience -= _xpToLevelUp;
            _xpToLevelUp += _level;

            _curOrtoSize = Mathf.Lerp(_orthoSizeMinMax.x, _orthoSizeMinMax.y, (float)_level / _maxOrthoLevel);
            _nextOrtoSize = Mathf.Lerp(_orthoSizeMinMax.x, _orthoSizeMinMax.y, ((float)_level + 1) / _maxOrthoLevel);

            _level +=1;

            levelText.text = _level.ToString();

            float spawnTime = enemySpammer.GetSpawnTime();
            enemySpammer.SetSpawnTime(spawnTime * spawnTimeDecrement);
            _changeOrthoSize = true;
            SpawnGifts();
        }

    }

    private void Update()
    {
        if (_changeOrthoSize)
        {
            _orthoSizeChangeTimer += Time.deltaTime;

            float t = _orthoSizeChangeTimer/ _orthoSizeChangeTime;
            float ortho = Mathf.Lerp(_curOrtoSize, _nextOrtoSize, t);
            _mainCam.orthographicSize = ortho;
            if (t >= 1)
            {
                print("ortho done");
                _orthoSizeChangeTimer = 0;
                _changeOrthoSize = false;
            }
        }
    }

    void SpawnGifts()
    {
        gifts.Clear();
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 10));
        float distance = 0.0f;
        Vector3 midPoint = Vector3.zero;

        if (_plane.Raycast(ray, out distance))
        {
            //Get mid point
            midPoint = ray.GetPoint(distance);
        }
        
        for (int i = 0; i< _level; i++)
        {
            /* Distance around the circle */
            var radians = 2 * MathF.PI / _level * i;

            /* Get the vector direction */
            var vertical = MathF.Sin(radians);
            var horizontal = MathF.Cos(radians);

            var spawnDir = new Vector3(horizontal, 0, vertical);

            /* Get the spawn position */
            float giftSpawnRadius = Mathf.Lerp(_giftSpawnRadiusMinMax.x, _giftSpawnRadiusMinMax.y, (float)_level / _maxOrthoLevel);
            var spawnPos = midPoint + spawnDir * giftSpawnRadius; // Radius is just the distance away from the point

            /* Now spawn */
            var gift = Instantiate(_giftPrefab, spawnPos, Quaternion.identity);

            /* Rotate the enemy to face towards player */
            gift.transform.LookAt(midPoint);
            gifts.Add(gift);
        }
    }
}
