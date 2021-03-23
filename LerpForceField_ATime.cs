using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpForceField_ATime : MonoBehaviour
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
    private int makeChangeTotalNum = 0;
    private int dirNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        forceField = new Vector2(0f, -1 * forceFieldScale);
        rb.gravityScale = 0f;
        dirNum = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !lerping)
        {
            makeChangeTotalNum += 1;
            dirNum = makeChangeTotalNum % 4;

            forceFieldLerpCountDown = forceFieldLerpTime;
            timeForceFieldStartLerp = Time.fixedTime;

            lerping = true;
        }
    }
    private void FixedUpdate()
    {
        switch (dirNum)
        {
            case 1:
                forceDir = new Vector2(-1f, 0f);
                break;
            case 2:
                forceDir = new Vector2(0f, 1f);
                break;
            case 3:
                forceDir = new Vector2(1f, 0f);
                break;
            default:
                forceDir = new Vector2(0f, -1f);
                break;
        }

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

    // Makes changing field more smoothy.
    public float fixedLerp(float start, float end, float timeStartLerp, float lerpTime)
    {
        float timeSinceStart = Time.fixedTime - timeStartLerp;
        float percentageComplete = timeSinceStart / lerpTime;

        var result = Mathf.Lerp(start, end, percentageComplete);

        return result;
    }
}
