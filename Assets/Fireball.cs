using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback 
{
    public Rigidbody myRb;
    public Transform myTransform;
    public float fbFwdForce = 1000;

    public Color myColor;
    public Color myOrange = new Color(255.0f, 80.0f, 0.0f);
    public Color myRed = new Color(129.0f, 19.0f, 0.0f);
    public Color myBlack = new Color(52.0f, 8.0f, 0.0f);

    public AudioClip fireSound;

    private bool red;
    private bool orange;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (myColor == myOrange)
            orange = true;
        if (myColor == myRed)
            red = true;
        if (!orange)
        {
            myColor = Color.Lerp(myColor, myOrange, 0.1f);
        }
        else if (!red)
        {
            myColor = Color.Lerp(myColor, myRed, 0.1f);
        }
        else
        {
            myColor = Color.Lerp(myColor, myBlack, 0.1f);
        }

        this.GetComponent<Renderer>().material.color = myColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag != "FireballProjectile" && tag != "Projectile")
        {
            //Color col = new Color(0.2F, 0.3F, 0.4F);
            myRb = GetComponent<Rigidbody>();
            AudioSource.PlayClipAtPoint(fireSound, myTransform.position);

            if (tag == "Barrier")
            {
                myRb.velocity = myRb.velocity / 100.0f;
            }
            if (tag == "Avatar")
            {
                Avatar aang = GetAvatar(other.gameObject, 0);
                int id = aang.id;
                int hitCt = (int)PhotonNetwork.LocalPlayer.Get(id).CustomProperties["HitCount"] + 1;
                PhotonHashtable ht = new PhotonHashtable();
                ht.Add("HitCount", hitCt);
                PhotonNetwork.LocalPlayer.Get(id).SetCustomProperties(ht, null, null);
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        myRb = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        myTransform.gameObject.tag = "FireballProjectile";
        myTransform.Rotate(100.0f, 0.0f, 0.0f, Space.Self);
        myColor = new Color(103.0f, 15.0f, 0.0f);
        this.GetComponent<Renderer>().material.color = myColor;
        red = false;
        orange = false;
        myRb.AddForce(myTransform.up * fbFwdForce);
        Destroy(this.gameObject, 4.0f);
    }

    public Avatar GetAvatar(GameObject go, int iter)
    {
        Avatar aang = go.GetComponent<Avatar>();
        if (aang == null && iter < 5)
        {
            aang = GetAvatar(go.transform.parent.gameObject, iter++);
        }
        return aang;
    }
}
