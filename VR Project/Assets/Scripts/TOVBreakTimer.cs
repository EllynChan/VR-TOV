using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOVBreakTimer : MonoBehaviour
{
    public GameObject next;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowNext());
    }
    IEnumerator ShowNext()
        {
            yield return new WaitForSecondsRealtime(10);

            next.SetActive(true);

        }

}
