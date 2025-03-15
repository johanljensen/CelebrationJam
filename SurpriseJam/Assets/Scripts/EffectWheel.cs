using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EffectWheel : MonoBehaviour
{
    [SerializeField] private Transform theWheel;
    
    [SerializeField] private float spinTime;
    private float spinTimer;
    
    [SerializeField] private float spinSpeed;
    private float curSpinSpeed;

    private bool animationDone;
    
    [SerializeField] private Transform arrowBase;

    [SerializeField] private AnimationCurve spinSpeedCurve;

    [SerializeField] private bool playerFriendly;
    [SerializeField] private Color goodColour;
    [SerializeField] private Color badColour;
    
    [SerializeField] private AnimationCurve spawnScaleCurve;
    [SerializeField] private AnimationCurve spawnBounceCurve;
    private float spawnTimer;

    [SerializeField] private float fadeOutTime;

    private Transform activatorTransform;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spinTimer = 0;
        curSpinSpeed = 0;

        spawnTimer = 0;
        
        animationDone = false;
        
        arrowBase.Rotate(Vector3.up, Random.Range(0f, 360f));

        if (activatorTransform == null)
        {
            Destroy(gameObject);
        }
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
        
        if (!animationDone)
        {
            float mappedSpeed = spinSpeedCurve.Evaluate(spinTimer / spinTime);
            curSpinSpeed = spinSpeed * (1 - mappedSpeed);

            arrowBase.Rotate(Vector3.up, curSpinSpeed);
            spinTimer += Time.deltaTime;

            if (curSpinSpeed == 0)
            {
                animationDone = true;

                ActivateEffect();
                DeleteME();
            }
        }
    }

    public void GoodWheel(bool isPlayerFriendly, Transform activator)
    {
        playerFriendly = isPlayerFriendly;
        activatorTransform = activator;
        Debug.Log("Recorded activator: " + activatorTransform.name);

        Material wheelMat = theWheel.GetComponentInChildren<SpriteRenderer>().material;
        wheelMat.color = isPlayerFriendly ? goodColour : badColour;
    }

    private void ActivateEffect()
    {
        EffectManager eff_manager = FindFirstObjectByType<EffectManager>();
        if (eff_manager != null)
        {
            eff_manager.ActivateRandomEffect(playerFriendly, activatorTransform, transform.position);
        }
    }
    
    private void DeleteME()
    {
        SpriteRenderer[] babyMeshes = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer babyMesh in babyMeshes)
        {
            StartCoroutine(FadeOut(babyMesh));
        }
    }

    IEnumerator FadeOut(SpriteRenderer meshRenderer)
    {
        Material datMat = meshRenderer.material;
        float fadeTimer = 0;
        
        while (fadeTimer < fadeOutTime)
        {
            float fadeFactor = 1 - fadeTimer / fadeOutTime;
            
            datMat.color = new Color(
                datMat.color.r,
                datMat.color.g,
                datMat.color.b,
                fadeFactor);

            fadeTimer += Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
