using System;
using UnityEngine;
using UnityEngine.UI;

public class ManaComponent : MonoBehaviour
{
    [Header("Mana Properties")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana = 100f;


    // Event that fires when mana changes
    public event Action<float, float> OnManaChanged; // currentMana, maxMana

    // Properties
    public float CurrentMana => currentMana;
    public float MaxMana => maxMana;

    private void Start()
    {

    }

    private void Update()
    {
        /*// Regenerate mana after delay
        //if ()
        //{
        //    AddMana();
        }*/
    }

    /// <summary>
    /// Use mana if there's enough available
    /// </summary>
    /// <param name="amount">Amount of mana to use</param>
    /// <returns>True if mana was successfully used</returns>
    public bool UseMana(float amount)
    {
        if (amount <= 0)
            return true;

        if (currentMana >= amount)
        {
            currentMana -= amount;

            OnManaChanged?.Invoke(currentMana, maxMana);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Add mana to the current amount
    /// </summary>
    /// <param name="amount">Amount of mana to add</param>
    public void AddMana(float amount)
    {
        if (amount <= 0)
            return;

        currentMana = Mathf.Min(currentMana + amount, maxMana);

        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    /// <summary>
    /// Set max mana and optionally refill current mana
    /// </summary>
    public void SetMaxMana(float newMax, bool refillCurrent = false)
    {
        maxMana = Mathf.Max(1, newMax);

        if (refillCurrent)
        {
            currentMana = maxMana;
        }
        else
        {
            // Ensure current mana doesn't exceed new max
            currentMana = Mathf.Min(currentMana, maxMana);
        }

        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    //public void CastSpell<TSpell>(TSpell spell) where TSpell : SpellBase
    //{
    //    Debug.Log($"Q triggered: {spell.SpellName}");

    //    if (spell == null)
    //    {
    //        Debug.Log("No spell selected");
    //        return;
    //    }
    //    spell.Cast(this.gameObject);
    //}
}