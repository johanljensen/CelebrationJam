using System;
using UnityEngine;

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

    private void Start()
    {
        _mainCam = Camera.main;
        _mainCam.orthographicSize = _orthoSizeMinMax.x;
    }

    public void EnemyDied()
    {
        _experience += 1;
        if (_experience >= _xpToLevelUp)
        {
            _experience -= _xpToLevelUp;
            _xpToLevelUp += _level;

            _curOrtoSize = Mathf.Lerp(_orthoSizeMinMax.x, _orthoSizeMinMax.y, (float)_level / _maxOrthoLevel);
            _nextOrtoSize = Mathf.Lerp(_orthoSizeMinMax.x, _orthoSizeMinMax.y, ((float)_level+1) / _maxOrthoLevel);

            _level +=1;
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
       /* for(int i = 0; i< experience; i++)
        {
            var gift = Instantiate(giftPrefab);
            //Spread out algorithm
        }*/
    }
}
