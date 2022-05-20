using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargettingGrenadeSeperateCase : MonoBehaviour
{
    public float SeperateHeight;
    public float SeperateAngel=30;
    public GameObject ChildGrenadeModel;
    public bool IsActive;

    private GameObject go1 = null, go2 = null;
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < SeperateHeight&& rb2D.velocity.y<0&&IsActive) {

            float speed = rb2D.velocity.magnitude;
            float angle = Mathf.Rad2Deg * Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) - SeperateAngel * 0.5f;
            if(go1 == null) {
                go1 = Instantiate(ChildGrenadeModel);
            }
            go1.SetActive(true);
            if (go1.GetComponent<TargettingGrenadeSeperateCase>() != null) {

                Destroy(go1.GetComponent<TargettingGrenadeSeperateCase>());
            }
            go1.transform.position = transform.position;
            go1.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * speed;
            angle += SeperateAngel;
            if (go2 == null)
            {
                go2 = Instantiate(ChildGrenadeModel);
            }
            go2.SetActive(true);
            if (go2.GetComponent<TargettingGrenadeSeperateCase>() != null)
            {
                Destroy(go2.GetComponent<TargettingGrenadeSeperateCase>());
            }
            go2.transform.position = transform.position;
            go2.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)) * speed;
            gameObject.SetActive(false);
        }
    }
}
