using UnityEngine;

public class E_LightBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private Rigidbody rb;

    private bool _spellGo;

    private void Update()
    {
        if (_spellGo)
        {
            rb.AddForce(transform.forward * speed); 
            lifetime -= Time.deltaTime;
            if (lifetime < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void Fire()
    {
        _spellGo = true;
    }
}
