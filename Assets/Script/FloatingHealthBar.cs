using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
   
    public void UpdateHealthBar(float currentvalue, float maxValue)
    {
        slider.value = currentvalue / maxValue;

    }
  

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    // public void SetHealth(float health, float maxHealth)
    // {
    //     Slider.gameObject.SetActive(health < maxHealth);
    //     Slider.value = health;
    //     Slider.maxValue = maxHealth;
        
    //     Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    //     Debug.Log(health);
    // }
    // // Update is called once per frame
    // void Update()
    // {
    //     Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
    // }
}
