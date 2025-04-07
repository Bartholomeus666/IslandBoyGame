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
        this.transform.parent = null;


        Debug.Log(transform.forward);

        rb.AddForce(this.transform.forward * speed, ForceMode.Impulse);

    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
