using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBeam : MonoBehaviour
{
    public Transform beam;
    public Transform ap;
    public Transform laser;
    public Transform holdpos;
    Rigidbody arb;

    // Start is called before the first frame update
    void Start()
    {
        ap = Instantiate<GameObject>(new GameObject(), this.transform).GetComponent<Transform>();
        beam.gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        laser.gameObject.GetComponent<Renderer>().enabled = false;
        float btnVal = Input.GetAxis("Oculus_GearVR_RGripTrigger");
        Ray r = new Ray(beam.position, beam.up);
        float distance = 3.0f;
        RaycastHit[] hits = Physics.RaycastAll(r, distance);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            Rigidbody rb = hit.rigidbody;
            if (rb != null)
            {
                if (btnVal > 0.5f && arb == null)
                {
                    arb = rb;
                    ap.position = this.transform.position;
                    ap.rotation = this.transform.rotation;
                    ap.localPosition = ap.localPosition + new Vector3(0.0f, 1.4f, 4.0f);
                }
                beam.transform.localScale = new Vector3(0.01f, hits[i].distance, 0.01f);
                laser.gameObject.GetComponent<Renderer>().enabled = true;
                break;
            }
        }
        if (btnVal < 0.4f)
        {
            arb = null;
        }
        
    }

    void FixedUpdate()
    {
        if (arb != null)
        {
            Vector3 difference = arb.position - ap.position;
            arb.velocity = -difference / Time.fixedDeltaTime;
            Quaternion rotationDifference = arb.rotation * Quaternion.Inverse(ap.rotation);
            float angle;
            Vector3 axis;
            rotationDifference.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = -Mathf.Deg2Rad * angle / Time.fixedDeltaTime * axis;
            arb.angularVelocity = angularVelocity;
        }
    }
    /*
    private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRb = other.attachedRigidbody;
        if (otherRb == null) return;
        float btnVal = Input.GetAxis("Oculus_GearVR_RGripTrigger");
        if (btnVal > 0.5f && arb == null)
        {
            arb = otherRb;
            arb.maxAngularVelocity = Mathf.Infinity;
            ap.position = arb.position;
            ap.rotation = arb.rotation;
            //this.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (btnVal < 0.2f && arb != null)
        {
            arb = null;
            //this.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    */
}
