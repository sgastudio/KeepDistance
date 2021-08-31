using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestory : MonoBehaviour
{
    public float duration;
    float startTime;
    public ParticleSystem effect;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SelfDestruction");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(duration);
        if (effect)
        {
            effect.transform.SetParent(null);
            effect.Play();
        }
        GameObject.Destroy(this.gameObject);
    }
}
