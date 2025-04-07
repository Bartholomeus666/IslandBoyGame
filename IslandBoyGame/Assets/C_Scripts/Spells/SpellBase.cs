using System.Collections;
using UnityEngine;

public abstract class SpellBase : ScriptableObject
{
    [Header("Spell Properties")]
    [SerializeField] protected float manaCost = 10f;
    [SerializeField] protected float castTime = 1.5f;
    [SerializeField] protected string spellName = "Base Spell";

    protected bool isCasting = false;

    // Properties for external access
    public float ManaCost => manaCost;
    public float CastTime => castTime;
    public string SpellName => spellName;
    public bool IsCasting => isCasting;

    public virtual bool Cast(GameObject caster)
    {
        if (isCasting)
        {
            return false;
        }

        // Check if caster has enough mana
        ManaComponent manaComponent = caster.GetComponentInParent<ManaComponent>();
        if (manaComponent != null && manaComponent.CurrentMana < manaCost)
        {
            Debug.Log($"Not enough mana to cast {spellName}");
            return false;
        }

        Debug.Log($"Casting {spellName}");

        // Delegate coroutine execution to a MonoBehaviour
        MonoBehaviour casterMono = caster.GetComponent<MonoBehaviour>();
        if (casterMono != null)
        {
            casterMono.StartCoroutine(CastRoutine(caster));
        }
        else
        {
            Debug.LogError($"Caster {caster.name} does not have a MonoBehaviour component to run coroutines!");
            return false;
        }

        return true;
    }

    /// <summary>
    /// The casting routine that handles timing for cast
    /// </summary>
    protected virtual IEnumerator CastRoutine(GameObject caster)
    {
        isCasting = true;

        // Begin cast animation or effects
        OnCastStart(caster);

        // Wait for cast time
        yield return new WaitForSeconds(castTime);

        // Cast is complete, apply effects
        ManaComponent manaComponent = caster.GetComponentInParent<ManaComponent>();
        if (manaComponent != null)
        {
            manaComponent.UseMana(manaCost);
        }

        OnCastComplete(caster);

        isCasting = false;
    }

    protected virtual void OnCastStart(GameObject caster)
    {
        Debug.Log($"Started casting {spellName}");
    }

    protected abstract void OnCastComplete(GameObject caster);

    public virtual bool InterruptCast(GameObject caster)
    {
        if (!isCasting)
            return false;

        // Stop coroutine via MonoBehaviour
        MonoBehaviour casterMono = caster.GetComponent<MonoBehaviour>();
        if (casterMono != null)
        {
            casterMono.StopCoroutine(CastRoutine(caster));
        }

        isCasting = false;
        Debug.Log($"{spellName} cast interrupted");
        return true;
    }
}
