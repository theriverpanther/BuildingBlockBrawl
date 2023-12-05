using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBarSprite;

    [SerializeField] private float reduceSpeed = 2;

    private float target = 1;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// Updates health bar based on the unit's current and maximum health
    /// </summary>
    /// <param name="maxHealth"></param>
    /// <param name="currentHealth"></param>
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
    }

    private void Update()
    {
        //Makes the health bar always face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthBarSprite.fillAmount = Mathf.MoveTowards(healthBarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
    
    public void SetBasicInfo(string tag, string unitName)
    {
        transform.GetChild(0).GetChild(0).GetComponent<Image>().color = tag == "PlayerCharacter" ? Color.green : Color.red;
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unitName;
    }

}
