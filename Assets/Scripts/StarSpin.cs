using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpin : MonoBehaviour
{
    private Transform star;
    // Start is called before the first frame update
    void Start()
    {
        star = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float degreeesPerSecond = 60;
        transform.Rotate(new Vector3(0, 0, -degreeesPerSecond) * Time.deltaTime);
    }
}
