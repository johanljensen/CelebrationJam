using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Transform effectTextPrefab;
    
    [SerializeField] private List<Transform> effectPrefabs;

    public void ActivateRandomEffect(bool playerFriendly, Transform activatorTransform, Vector3 wheelPosition)
    {
        Transform randomEffect = effectPrefabs[Random.Range(0, effectPrefabs.Count)];
        randomEffect = Instantiate(randomEffect);
        
        Effect theEffect = randomEffect.GetComponent<Effect>();
        theEffect.ActivateEffect(playerFriendly, activatorTransform, wheelPosition);
        
        Transform effectText = Instantiate(effectTextPrefab, wheelPosition, Quaternion.identity);
        effectText.GetComponent<EffectText>().SetText(theEffect.GetName());
        effectText.position = wheelPosition;
    }
}
