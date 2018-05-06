using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DamageCanvas : MonoBehaviour
{
    private int Damage;

    public float Distance;

    private Text text;
    private bool isfading;

    private Transform cam;


    private void Start()
    {
        text = GetComponent<Text>();

        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }

        Destroy(gameObject, 5);
    }

    
    private void Update()
    {
        //Face towards player
        var v = transform.position - cam.position;
        transform.rotation = Quaternion.LookRotation(v.normalized);
    }

    /// <summary>
    /// new hit passthrough for displaying the hit at the top of a capsule collider
    /// </summary>
    /// <param name="c">The collider </param>
    /// <param name="Damage">The damage</param>
    /// <param name="wasCrit">if the hit was a crit</param>
    public void NewHit(CapsuleCollider c, int Damage, bool wasCrit)
    {
        if (!c)
        {
            Debug.Log("No capsule found, not displaying hitmarker");
            return;
        }

        NewHit(c.bounds.center + (Vector3.up * c.height/2), Damage, wasCrit);
    }

    public void NewHit(Vector3 pos, int Damage, bool wasCrit)
    {        
        //Sanity check
        if (isfading)
        {
            Debug.Log("Already fading!");
            return;
        }

        //Initalize if we haven't already
        if (!text) Start();

        transform.position = pos; //set pos
        text.text = Damage.ToString(); //set damage
        if (wasCrit) text.text += "!"; //add exclamation if crit

        StartCoroutine(MoveDatAss(3));

    }

    /// <summary>
    /// Divies the animation up into 3 sections
    /// </summary>    
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator MoveDatAss(float duration)
    {
        //Fade in
        float elapsed1 = 0;
        Vector3 startPos = transform.position;

        Color c = text.color;
        c.a = 0;
        text.color = c;
        while (elapsed1 < duration/3)
        {
            var t = elapsed1 / (duration/3);
            text.color = new Color(c.r, c.g, c.b, t);
            transform.position = startPos + (Vector3.up * Mathf.Sin(t) * 0.5f);
            elapsed1 += Time.deltaTime;
            yield return null;
        }

        //Linger
        yield return new WaitForSeconds(duration / 3);

        //Fade out
        float elapsed2 = 0;
        while (elapsed2 < duration / 3)
        {
            var t = elapsed2 / (duration / 3);
            text.color = new Color(c.r, c.g, c.b, 1 - t);
            elapsed2 += Time.deltaTime;
            yield return null;
        }
    }
}
