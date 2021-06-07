using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeObject : MonoBehaviour
{
    Rigidbody2D rb2;

    void Awake()
    {
        Debug.Log("ВЫЗВАЛИ ФЭЙКОВЫЙ ОБЪЕКТ");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((transform.rotation.z > 0f) | (transform.rotation.z < 180f)) & ((transform.rotation.z > -180f) | (transform.rotation.z < -360f)))
        {
            transform.position = new Vector3(transform.position.x - 3f, transform.position.y, transform.position.z);
            Transform cen = transform.Find("Center");
            rb2.centerOfMass = cen.localPosition;
        }
        if (((transform.rotation.z < 0f) | (transform.rotation.z > -180f)) & ((transform.rotation.z > 180f) | (transform.rotation.z < 360f)))
        {
            transform.position = new Vector3(transform.position.x + 3f, transform.position.y, transform.position.z);
            Transform cen = transform.Find("Center");
            rb2.centerOfMass = cen.localPosition;
        }
    }

}
