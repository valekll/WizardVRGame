using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayer : MonoBehaviour
{
    public Rig rig;

    public Transform myTransform;
    public bool ignoreX = false;
    public bool ignoreY = false;
    public bool ignoreZ = false;
    public float knockback = 1.0f;

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
            rig.head.Translate(tVelocity.normalized * ukb, Space.World);
            rig.lHand.Translate(tVelocity.normalized * ukb, Space.World);
            rig.rHand.Translate(tVelocity.normalized * ukb, Space.World);
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
            /*
            string nname = PhotonNetwork.LocalPlayer.NickName;
            string[] nameSplit = nname.Split(' ');
            int hitCt = int.Parse(nameSplit[nameSplit.Length - 1]) + 1;
            nname = "";
            for(int i = 0; i < nameSplit.Length - 2; i++)
            {
                nname += nameSplit[i];
            }
            PhotonNetwork.LocalPlayer.NickName = nname + hitCt;
            */
        }
    }
}
