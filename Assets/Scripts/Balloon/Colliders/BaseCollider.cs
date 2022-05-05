using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollider : MonoBehaviour
{
    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Base");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = target.transform.position;
        gameObject.transform.rotation = target.transform.rotation;
    }
}
