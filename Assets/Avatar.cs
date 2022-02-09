using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rig rig;
    public Transform head;
    public Transform lHand;
    public Transform rHand;
    public Transform body;

    public Vector3 headNetworkPos;
    public Vector3 lHandNetworkPos;
    public Vector3 rHandNetworkPos;
    public Quaternion headNetworkRot;
    public Quaternion lHandNetworkRot;
    public Quaternion rHandNetworkRot;

    public string name;
    public int id;

    // Start is called before the first frame update
    private void Start()
    {
        if (photonView.IsMine)
        {
            //head.gameObject.SetActive(false);
            //body.gameObject.SetActive(false);
            head.gameObject.GetComponent<Renderer>().enabled = false;
            body.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            head.position = rig.head.position;
            head.rotation = rig.head.rotation;
            body.position = rig.head.position - new Vector3(0.0f, 0.5f, 0.0f);
            lHand.position = rig.lHand.position;
            lHand.rotation = rig.lHand.rotation;
            rHand.position = rig.rHand.position;
            rHand.rotation = rig.rHand.rotation;
        }
        else
        {
            head.position = Vector3.Lerp(head.position, headNetworkPos, 0.2f);
            head.rotation = Quaternion.Lerp(head.rotation, headNetworkRot, 0.2f);

            body.position = Vector3.Lerp(body.position, headNetworkPos - new Vector3(0.0f, 0.5f, 0.0f), 0.2f);

            lHand.position = Vector3.Lerp(lHand.position, lHandNetworkPos, 0.2f);
            lHand.rotation = Quaternion.Lerp(lHand.rotation, lHandNetworkRot, 0.2f);

            rHand.position = Vector3.Lerp(rHand.position, rHandNetworkPos, 0.2f);
            rHand.rotation = Quaternion.Lerp(rHand.rotation, rHandNetworkRot, 0.2f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rig.head.position);
            stream.SendNext(rig.head.rotation);
            stream.SendNext(rig.lHand.position);
            stream.SendNext(rig.lHand.rotation);
            stream.SendNext(rig.rHand.position);
            stream.SendNext(rig.rHand.rotation);
        }
        else
        {
            headNetworkPos = (Vector3)stream.ReceiveNext();
            headNetworkRot = (Quaternion)stream.ReceiveNext();

            lHandNetworkPos = (Vector3)stream.ReceiveNext();
            lHandNetworkRot = (Quaternion)stream.ReceiveNext();

            rHandNetworkPos = (Vector3)stream.ReceiveNext();
            rHandNetworkRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
