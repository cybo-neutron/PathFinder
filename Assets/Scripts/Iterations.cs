using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iterations : MonoBehaviour
{

    public List<string> names;

    private void Start()
    {
        StartCoroutine(PrintNames());
    }
    IEnumerator PrintNames()
    {
        yield return null;

        foreach (var name in names)
        {
            print(name);

            yield return new WaitForSeconds(1f);
        }
    }

}
