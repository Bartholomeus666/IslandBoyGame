using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100;
    public float currentHealth = 100;
    private GameObject healthVignettePanel;

    private void Start()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();

        if (canvas != null)
        {
            healthVignettePanel = canvas.transform.Find("BloodVignette").gameObject;
        }
    }

    public void LoseHealth(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        CheckIfLowHealth();
    }

    public void GainHealth(float heals)
    {
        currentHealth += heals;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        CheckIfLowHealth();
    }

    private void CheckIfLowHealth()
    {
        if (currentHealth < 0)
        {
            Debug.Log("Death");
        }
        else if (currentHealth < 20)
        {
            Debug.Log("Bloody vignette");
            healthVignettePanel.gameObject.SetActive(true);
        }
        else if (currentHealth > 20)
        {
            healthVignettePanel.gameObject.SetActive(false);
        }
    }
}
