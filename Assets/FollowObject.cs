using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;
    public float speed = 50.0f;
    public float yOffset = 3.0f;
    public float zOffset = 3.0f;
    public float xOffset = 0.0f;

    private float deltaScroll;

    // Start is called before the first frame update
    void Start()
    {
        //transform.Rotate(-20.0f, 0, 0, Space.World);
        StartCoroutine(followTarget());
    }

    IEnumerator followTarget()
    {
        while (true)
        {
            /*
            deltaScroll = Input.GetAxis("Mouse ScrollWheel");
            if (deltaScroll > 0.0f)
            {
                yOffset -= 0.1f;
                zOffset -= 0.1f;
            }
            else if(deltaScroll < 0.0f)
            {
                yOffset += 0.1f;
                zOffset += 0.1f;
            }

            if (yOffset < 1.7f)
                yOffset = 1.7f;
            else if (yOffset > 10.0f)
                yOffset = 10.0f;
            if (zOffset < 1.7f)
                zOffset = 1.7f;
            else if (zOffset > 10.0f)
                zOffset = 10.0f;
            */

            Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
            Vector3 waypointPos = target.position + offset;
            
            Vector3 toWaypoint = waypointPos - transform.position;
            Vector3 direction = toWaypoint.normalized;
            Vector3 toMove = direction * speed * Time.deltaTime;

            if (toWaypoint.magnitude < toMove.magnitude)
            {
                toMove = toWaypoint;
            }

            transform.position = transform.position + toMove;

            yield return null;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
