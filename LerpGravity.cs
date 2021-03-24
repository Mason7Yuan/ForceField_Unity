using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpGravity : MonoBehaviour
{
    private Rigidbody2D rb;
    private float timeGravityStartLerp;
    private float gravityLerpCountDown;

    [SerializeField] private float gravityLerpTime = .5f;                   // Minus 1 for 0 to negative regime.
    private float gravityScaleStart;
    private float gravityScaleEnd;
    [Range(.5f, 2f)] [SerializeField] private float gravityMultiplyer = 1;

    private float angleStart;
    private float angleEnd;
    private float angleLerpCountDown;                                       // Coordinates with gravityLerpTime.
    private float timeAngleStartLerp;
    [SerializeField] private float angleLerpTime = .3f;                      // Value is larger than gravityLerpTime.

    private bool lerping = false;
    private bool angleLerping = false;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityScaleStart = rb.gravityScale;
        gravityScaleEnd = (-1) * gravityScaleStart;
        angleStart = 0f;
        angleEnd = 180f;

        // Freezing rotation (z-axis).
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !angleLerping && !lerping)
        {
            gravityLerpCountDown = gravityLerpTime;
            timeGravityStartLerp = Time.fixedTime;
            angleLerpCountDown = angleLerpTime;
            timeAngleStartLerp = Time.fixedTime;

            lerping = true;
            angleLerping = true;
        }
    }

    private void FixedUpdate()      // Time.fixedDeltaTime == 0.02f;
    {
        if (gravityLerpCountDown >= 0f && lerping)
        {
            rb.gravityScale = fixedLerp(gravityScaleStart, gravityScaleEnd, timeGravityStartLerp, gravityLerpTime);
            gravityLerpCountDown -= Time.fixedDeltaTime;
        }
        else if (lerping)
        {
            gravityScaleStart = rb.gravityScale;
            gravityScaleEnd = (-1) * gravityMultiplyer * gravityScaleStart;
            lerping = false;

            Flip();
        }
        
        if (angleLerpCountDown >= 0f && angleLerping)
        {
            transform.eulerAngles = new Vector3(0, 0, fixedLerp(angleStart, angleEnd, timeAngleStartLerp, angleLerpTime));
            angleLerpCountDown -= Time.fixedDeltaTime;
        }
        else if (angleLerping)
        {
            angleStart = (-1) * transform.eulerAngles.z;
            angleEnd = angleStart + 180f;
            angleLerping = false;
        }
    }

    public float fixedLerp(float start, float end, float timeStartLerp, float lerpTime)
    {
        float timeSinceStart = Time.fixedTime - timeStartLerp;
        float percentageComplete = timeSinceStart / lerpTime;

        var result = Mathf.Lerp(start, end, percentageComplete);

        return result;
    }

    // avatar flip as gravity change direction.
    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
