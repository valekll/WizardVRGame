using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Transform[] myTransforms;
    public Transform myTransform;
    public bool ignoreX = false;
    public bool ignoreY = false;
    public bool ignoreZ = false;
    public float knockback = 3.0f;

    private float kbDistance;
    private Vector3 tVelocity;

    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        kbDistance = knockback;
    }

    // Update is called once per frame
    void Update()
    {
        if (kbDistance > 0)
        {
            float ukb = knockback * Time.deltaTime;
            if (ukb > kbDistance)
                ukb = kbDistance;
            for (int i = 0; i < myTransforms.Length; i++)
            {
                myTransforms[i].Translate(tVelocity.normalized * ukb, Space.World);
            }
            myTransform.Translate(tVelocity.normalized * ukb, Space.World);
            kbDistance -= ukb;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FireballProjectile")
        {
            Rigidbody oRb = other.GetComponent<Rigidbody>();
            tVelocity = oRb.velocity;
            if (ignoreX) tVelocity.x = 0.0f;
            if (ignoreY) tVelocity.y = 0.0f;
            if (ignoreZ) tVelocity.z = 0.0f;

            kbDistance = knockback;
        }
    }
}
