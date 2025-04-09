using UnityEngine;

public class CharacterSpellCaster : MonoBehaviour
{
    public SpellBase equippedSpell;
    public GameObject Caster;

    public void CastSpell()
    {
        if (equippedSpell != null)
        {
            equippedSpell.Cast(Caster.gameObject);
        }
    }
}
