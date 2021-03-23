using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpForceField : MonoBehaviour
{
    private Rigidbody2D rb;
    private float timeForceFieldStartLerp;
    private float forceFieldLerpCountDown;
    private float forceFieldScale = 9.81f;
    private Vector2 forceField;
    private Vector2 forceDir;

    [SerializeField] private float forceFieldLerpTime = .5f;                        // Minus 1 for 0 to negative regime.
    private float forceFieldScaleStart = 9.81f;
    private float forceFieldScaleMid = 0f;
    [Range(.5f, 2f)] [SerializeField] private float forceFieldMultiplyer = 1f;      // Increase force field as change times increasing.

    private bool lerping = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        forceField = new Vector2(0f, -1 * forceFieldScale);
        rb.gravityScale = 0f;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !lerping)
        {
            forceDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            forceFieldLerpCountDown = forceFieldLerpTime;
            timeForceFieldStartLerp = Time.fixedTime;

            lerping = true;
        }
        else if (forceDir.x == 0 && forceDir.y == 0)
        {
            forceDir = new Vector2(0, -1f);
        }
    }
    private void FixedUpdate()
    {
        if (forceFieldLerpCountDown >= forceFieldLerpTime / 2 && lerping)
        {
            forceFieldScale = fixedLerp(forceFieldScaleStart, forceFieldScaleMid, timeForceFieldStartLerp, forceFieldLerpTime);
            forceFieldLerpCountDown -= Time.fixedDeltaTime;
        }
        else if (forceFieldLerpCountDown < forceFieldLerpTime / 2 && forceFieldLerpCountDown >= 0 && lerping)
        {
            forceFieldScale = fixedLerp(forceFieldScaleMid, forceFieldScaleStart, timeForceFieldStartLerp, forceFieldLerpTime);
            forceFieldLerpCountDown -= Time.fixedDeltaTime;
        }
        else if (lerping)
        {
            forceFieldMultiplyer += 0f;
            lerping = false;
        }

        forceField = forceDir * forceFieldScale * forceFieldMultiplyer;
        rb.AddForce(forceField);
    }

    public float fixedLerp(float start, float end, float timeStartLerp, float lerpTime)
    {
        float timeSinceStart = Time.fixedTime - timeStartLerp;
        float percentageComplete = timeSinceStart / lerpTime;

        var result = Mathf.Lerp(start, end, percentageComplete);

        return result;
    }
}
