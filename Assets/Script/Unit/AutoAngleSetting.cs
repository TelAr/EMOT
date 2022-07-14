using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAngleSetting : MonoBehaviour
{
    public float OffsetAngle = 0;
    public bool IsAutoAngle = false;
    public float RotatePerSecond = 0;

    private Rigidbody2D rb = null;
    private float rotateValue = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) {

            Debug.Log("Warning: AutoAngleSetting must need Rigidbody2D");
        }
    }

    private void OnEnable()
    {
        rotateValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = 0;
        rotateValue += Time.deltaTime * RotatePerSecond;
        if (rb != null && IsAutoAngle) {

            angle = Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x);

        }

        transform.rotation = Quaternion.Euler(0, 0, angle + OffsetAngle + rotateValue);
    }
}
