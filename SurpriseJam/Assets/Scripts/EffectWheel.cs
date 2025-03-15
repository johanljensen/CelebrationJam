using UnityEngine;
using UnityEngine.Serialization;

public class EffectWheel : MonoBehaviour
{
    [SerializeField] private Transform theWheel;
    
    [SerializeField] private float spinTime;
    private float spinTimer;
    
    [SerializeField] private float spinSpeed;
    private float curSpinSpeed;

    [SerializeField] private Transform arrowBase;

    [SerializeField] private AnimationCurve spinSpeedCurve;

    [SerializeField] private bool playerFriendly;
    [SerializeField] private Color goodColour;
    [SerializeField] private Color badColour;
    
    
    
    
    [SerializeField] private AnimationCurve spawnScaleCurve;
    [SerializeField] private AnimationCurve spawnBounceCurve;
    private float spawnTimer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spinTimer = 0;
        curSpinSpeed = 0;

        spawnTimer = 0;
        
        arrowBase.Rotate(Vector3.up, Random.Range(0f, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer < 1)
        {
            theWheel.localScale = Vector3.one * spawnScaleCurve.Evaluate(spawnTimer);
            theWheel.localPosition = Vector3.zero + Vector3.up * spawnBounceCurve.Evaluate(spawnTimer);
            spawnTimer += Time.deltaTime;
        }
        
        if (spinSpeed > 0)
        {
            float mappedSpeed = spinSpeedCurve.Evaluate(spinTimer / spinTime);
            curSpinSpeed = spinSpeed * (1 - mappedSpeed);

            arrowBase.Rotate(Vector3.up, curSpinSpeed);
            spinTimer += Time.deltaTime;
        }
    }

    public void GoodWheel(bool isPlayerFriendly)
    {
        playerFriendly = isPlayerFriendly;

        Material wheelMat = theWheel.GetComponentInChildren<SpriteRenderer>().material;
        wheelMat.color = isPlayerFriendly ? goodColour : badColour;
    }
}
