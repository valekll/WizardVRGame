using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCube : MonoBehaviour
{

    public float cubeSpeed;
    public bool curr;

    // Start is called before the first frame update
    void Start()
    {
        cubeSpeed = 2.0f;
        curr = false;
    }

    // Update is called once per frame
    void Update()
    {
        float fire1trigger = Input.GetAxis("Fire1");
        float fire2trigger = Input.GetAxis("Fire2");
        float fire3trigger = Input.GetAxis("Fire3");
        float jumptrigger = Input.GetAxis("Jump");
        float canceltrigger = Input.GetAxis("Cancel"); 
        if (fire1trigger > 0.5f) ChangeColor(Color.red);
        else if (fire2trigger > 0.5f) ChangeColor(Color.blue);
        else if (fire3trigger > 0.5f) ChangeColor(Color.green);
        else if (jumptrigger > 0.5f) ChangeColor(Color.yellow);
        else if (canceltrigger > 0.5f) ChangeColor(Color.black);

        float litrigger = Input.GetAxis("Oculus_GearVR_LIndexTrigger");
        //if (litrigger > 0.5f) ToggleColor(Color.grey, Color.white);

        float xmv = Input.GetAxis("Oculus_GearVR_LThumbstickX");
        float ymv = Input.GetAxis("Oculus_GearVR_LThumbstickY");
        transform.Translate(Time.deltaTime * cubeSpeed * xmv, Time.deltaTime * cubeSpeed * ymv, 0);
    }

    public void ChangeColor(Color cubeColor)
    {
        GetComponent<Renderer>().material.color = cubeColor;
    }

    public void ToggleColor(Color c1, Color c2)
    {
        if(!curr)
        {
            GetComponent<Renderer>().material.color = c1;
            curr = true;
        }
        else
        {
            GetComponent<Renderer>().material.color = c2;
            curr = false;
        }
    }
}
