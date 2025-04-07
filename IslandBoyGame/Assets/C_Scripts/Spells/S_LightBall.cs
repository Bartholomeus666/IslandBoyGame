using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Fire Spell", menuName = "Spells/LightBall Spell")]
public class S_LightBall : SpellBase
{
    [Header("LightBall Properties")]
    [SerializeField] private GameObject lightBallPrefab;
    private E_LightBall _lightBall;

    private void Awake()
    {
        // Set default values for the base spell
        spellName = "LightBall";
        
        manaCost = 15f;
        castTime = 1f;
    }

    public override bool Cast(GameObject caster)
    {
        return base.Cast(caster);
    }

    protected override void OnCastStart(GameObject caster)
    {
        base.OnCastStart(caster);

        // Basic light animation

        _lightBall = Instantiate(lightBallPrefab, caster.transform).GetComponent<E_LightBall>();
        Debug.Log("Light instantiated");
        
    }

    protected override void OnCastComplete(GameObject caster)
    {
        _lightBall.Fire();
    }
}
