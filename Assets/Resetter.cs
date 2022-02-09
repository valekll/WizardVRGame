using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetter : MonoBehaviour
{
    public Vector3 startPos;
    public Quaternion startRot;
    public float speed = 3.0f;
    public bool reset;

    public float target;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        startRot = this.transform.rotation;
        reset = false;
        target = Random.Range(0.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != startPos)
        {
            if (reset)
            {
                Vector3 waypointPos = startPos;

                Vector3 toWaypoint = waypointPos - transform.position;
                Vector3 direction = toWaypoint.normalized;
                Vector3 toMove = direction * speed * Time.deltaTime;

                if (toWaypoint.magnitude < toMove.magnitude)
                {
                    toMove = toWaypoint;
                }

                transform.rotation = Quaternion.RotateTowards(transform.rotation, startRot, speed * Time.deltaTime);
            }
            else
            {
                StartCoroutine(wait(5.0f));
            }
        }
        else
        {
            reset = false;
        }
        if (transform.position.y > 3.0f)
        {
            transform.position.y = 
        }
    }

    IEnumerator wait(float s)
    {
        yield return new WaitForSeconds(s);
        reset = true;
    }
}
