using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowCamera : MonoBehaviour
{
    public Transform camera;
    public float offsetDistance = 30;
    public float offx;
    public float offy;
    public float offz;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offsetxyz = new Vector3(offx, offy, offz);
        Vector3 offsetPos = (camera.position + camera.forward * offsetDistance) + offsetxyz;
        
        transform.position = Vector3.Lerp(transform.position, offsetPos, 0.2f);
        transform.rotation = Quaternion.Lerp(transform.rotation, camera.rotation, 0.2f);
    }
}
