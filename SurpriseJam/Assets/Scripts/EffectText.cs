using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;

    [SerializeField] private float textLifetime;
    [SerializeField] private float ascendSpeed;
    private float textLifetimer;
    
    private void Update()
    {
        if (textLifetimer < textLifetime)
        {
            transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
            textLifetimer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        myText.text = text;
    }
}
