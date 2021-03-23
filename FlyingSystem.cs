using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSystem : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 forceField;
    private Vector2 forceDir;

    private float forceFieldScale = 9.81f;
    private float canFlyingTime = 2f;
    private float flyingTime;
    private bool pushing;
    private bool addFuel;

    [Range(.5f, 2f)] [SerializeField] private float forceFieldMultiplyer = 1f;      // Increase force field as change times increasing.

    // Start is called before the first frame update
    // Initialize the values.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        forceField = new Vector2(0f, -1 * forceFieldScale);
        rb.gravityScale = .5f;                                      // Initailize the gravity force.
        flyingTime = canFlyingTime;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;     // Give a constraint on z axis.
    }

    // Update is called once per frame
    void Update()
    {
        pushing = Input.GetButton("Fire1");
        addFuel = Input.GetButtonDown("Fire2");

        if (pushing)
            forceDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); 
        else
            forceDir = new Vector2(0, -1f);

        if (addFuel)
        {
            canFlyingTime = flyingTime;
        }
        
    }
    private void FixedUpdate()
    {
        forceField = forceDir * forceFieldScale * forceFieldMultiplyer;
        
        if (canFlyingTime > 0 && pushing)
        {
            rb.AddForce(forceField);
            canFlyingTime -= Time.fixedDeltaTime;
        }
    }
}
