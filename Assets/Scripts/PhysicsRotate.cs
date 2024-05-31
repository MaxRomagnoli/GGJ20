using UnityEngine;

public class PhysicsRotate : MonoBehaviour
{
    [SerializeField] private float force = 10f;
    [SerializeField] private float forceLimit = 10f;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.AddTorque(force, ForceMode2D.Force);
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, 0, forceLimit);
    }
}
