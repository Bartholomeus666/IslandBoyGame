using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class SpellMenu : ScriptableObject
{
    public int spellId;
    public SpellBase SpellScript;
}
