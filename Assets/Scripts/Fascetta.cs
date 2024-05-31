using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fascetta : MonoBehaviour
{
    [Header("Fascetta")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    //[SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private BoxCollider2D startCollider;

    [Header("Fine fascetta")]
    [SerializeField] private Rigidbody2D endZipTieRb;
    //[SerializeField] private CircleCollider2D endZipTieCollider;
    [SerializeField] private BoxCollider2D endCollider;

    [Header("Variabili")]
    [SerializeField] private float zipForce = 500f;
    [SerializeField] private float zipForce2 = 100f;
    [SerializeField] private float minZip = 0.5f;
    [SerializeField] private float maxZipForce = 1f;
    bool isZipping = false;
    float lastZipLenght;

    // Start is called before the first frame update
    void Start()
    {
        startCollider.enabled = false;
        endCollider.enabled = false;
        //spriteRenderer.color = GetRandomColor();
    }

    //Color GetRandomColor()
    //{
    //    return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    //}

    public void SetColor(Color colorToSet)
    {
        spriteRenderer.color = colorToSet;
    }

    public void StartZip()
    {
        AudioManager.instance.Play("Zip");

        //abilita i box collider
        startCollider.enabled = true;
        endCollider.enabled = true;

        isZipping = true;
    }

    /*public void StopZip()
    {
        //mette nuovamente i rigidbody a cinematici
        rb.bodyType = RigidbodyType2D.Static;
        endZipTieRb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
        endZipTieRb.velocity = Vector2.zero;

        isZipping = false;
    }*/

    public void DestroyZip()
    {
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        if (isZipping)
        {
            if (lastZipLenght < minZip)
            {
                DestroyZip();
            } else
            {
                Vector2 forceDirection = (transform.position - endZipTieRb.transform.position).normalized;
                endZipTieRb.AddForce(forceDirection * Time.fixedDeltaTime * zipForce, ForceMode2D.Force);
                rb.AddForce(-forceDirection * Time.fixedDeltaTime * zipForce2, ForceMode2D.Force);
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxZipForce);
                endZipTieRb.velocity = Vector2.ClampMagnitude(endZipTieRb.velocity, maxZipForce);
                LookAtMouse(endZipTieRb.transform.position, false);
            }
        }
    }

    void SetZipLenght()
    {
        lastZipLenght = (transform.position - endZipTieRb.transform.position).magnitude;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, lastZipLenght);
    }

    public void LookAtMouse(Vector2 target, bool setTarget = true)
    {
        Vector3 myLocation = transform.position;
        Vector3 targetLocation = target;
        targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position

        // vector from this object towards the target location
        Vector3 vectorToTarget = targetLocation - myLocation;
        // rotate that vector by 90 degrees around the Z axis
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 0) * vectorToTarget;

        // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
        // (resulting in the X axis facing the target)
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        // changed this from a lerp to a RotateTowards because you were supplying a "speed" not an interpolation value
        transform.rotation = targetRotation;

        // set the zip tie lenght
        if (setTarget) { endZipTieRb.transform.position = target; }
        SetZipLenght();
    }
}
