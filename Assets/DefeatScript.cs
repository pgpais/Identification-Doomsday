using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatScript : MonoBehaviour
{
    public float timeUntilShot = 1.5f;
    public float timeAfterShot = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartSound()
    {
        yield return new WaitForSecondsRealtime(timeUntilShot);
        FMODUnity.RuntimeManager.PlayOneShot("event:/sfx/shot", transform.position);
        Debug.Log("DED");
        yield return new WaitForSecondsRealtime(timeAfterShot);
        Application.Quit();
    }
}
