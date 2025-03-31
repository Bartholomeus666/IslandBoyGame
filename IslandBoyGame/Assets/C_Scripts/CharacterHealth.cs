using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float health = 100;
    public Canvas HealthCanvas;

    public void LoseHealth(float damage)
    {
        health -= damage;
        CheckIfLowHealth();
    }

    public void GainHealth(float heals)
    {
        health += heals;
        CheckIfLowHealth();
    }

    private void CheckIfLowHealth()
    {
        if(health < 0)
        {
            Debug.Log("Death");
        }
        else if (health < 20)
        {
            Debug.Log("Bloody vignet"); 
            HealthCanvas.gameObject.SetActive(true);
        } 
    }
}
