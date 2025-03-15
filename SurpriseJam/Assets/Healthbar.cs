using System;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
   [SerializeField] private Transform _HeathImgTransform;
   private float _currentHealth;
   private float _maximumHealth;

   private void UpdateHealth()
   {
      float healthValue = _currentHealth/_maximumHealth;
      healthValue = Mathf.Clamp01(healthValue);
      _HeathImgTransform.localScale = new Vector3(healthValue, 1f, 1f);
   }

   public void SetHealth(float currentHealth, float maximumHealth)
   {
      _currentHealth = currentHealth;
      _maximumHealth = maximumHealth;
      UpdateHealth();
   }

}
