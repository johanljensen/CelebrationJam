using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EffectBox : MonoBehaviour
{

    [SerializeField] private Transform wheelPrefab;
    [SerializeField] private float fadeOutTime;

    bool noMoreTouching = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (noMoreTouching)
            return;

        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Debug.Log(other.transform.name + " touched me!");

            EffectWheel wheel = Instantiate(wheelPrefab, transform.position, Quaternion.identity).GetComponent<EffectWheel>();
            wheel.GoodWheel(other.GetComponent<PlayerController>() != null, other.transform);

            DeleteME();
            noMoreTouching = true;
        }
    }

    private void DeleteME()
    {
        MeshRenderer[] babyMeshes = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer babyMesh in babyMeshes)
        {
            StartCoroutine(FadeOut(babyMesh));
        }
    }

    IEnumerator FadeOut(MeshRenderer meshRenderer)
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
