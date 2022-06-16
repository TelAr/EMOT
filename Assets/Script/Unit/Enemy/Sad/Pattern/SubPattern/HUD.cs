using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public Color mainColor;

    public GameObject A, B, C;
    public float TransitionDelay;
    public bool Is_end = false;
    public float EffectSoundVolumeOffset = 1f;

    private GameObject target;
    private float timer;
    private Vector3 AS, BS, CS;

    private void OnEnable()
    {
        A.GetComponent<SpriteRenderer>().color = B.GetComponent<SpriteRenderer>().color = C.GetComponent<SpriteRenderer>().color = Color.clear;
    }

    private void Awake()
    {
        AS = A.transform.localScale;
        BS = B.transform.localScale;
        CS = C.transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initiate(GameObject Target=null) {

        if (target == null)
        {

            target = GameController.GetPlayer();
        }
        else {

            target = Target;
        }
        A.transform.localScale = B.transform.localScale = C.transform.localScale = Vector3.zero;
        timer = 0;
        Is_end = false;
        if (gameObject.GetComponent<AudioSource>() != null) {
            gameObject.GetComponent<AudioSource>().volume = EffectSoundVolumeOffset * AudioDefault.MasterVolume * AudioDefault.EffectVolume;
        }
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        //        gameObject.transform.position = target.transform.position;

        A.GetComponent<SpriteRenderer>().color = B.GetComponent<SpriteRenderer>().color = C.GetComponent<SpriteRenderer>().color = mainColor;
        if (timer < TransitionDelay)
        {
            if (!Is_end) {
                timer += Time.deltaTime;
            }

            //A
            if (timer < TransitionDelay * 0.5f)
            {
                A.transform.localScale = new Vector3(AS.x * (1 - Mathf.Pow(1 - timer * 2 / TransitionDelay, 2)), AS.y, AS.z);
            }
            else {

                A.transform.localScale = AS;
            }
            

            //B
            B.transform.localScale = new Vector3(BS.x * (1-Mathf.Pow(1-timer / TransitionDelay, 2)), BS.y, BS.z);

            //C
            C.transform.localScale = CS * (1 - Mathf.Pow(1 - timer / TransitionDelay, 2));
        }
        else {

            A.transform.localScale = AS;
            B.transform.localScale = BS;
            C.transform.localScale = CS;
        }


        if (Is_end) {

            timer -= Time.deltaTime;
            if (timer < 0) { 
            
                gameObject.SetActive(false);
            }
        }
    }
}
