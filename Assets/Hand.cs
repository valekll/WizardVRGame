using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviourPunCallbacks
{

    public Transform head;
    public Transform trackingSpace;
    public float speed;
    public Rigidbody attachedRigidBody;
    public Transform attachPoint;
    public Transform laser;
    public GameObject arcPointPrefab;
    public float arcSpeed;
    public float snapRotateDeltaSmall;
    public float snapRotateDeltaLarge;
    public bool canSnapRotate;
    public float rotateSpeed;
    public bool rightHand;

    public GameObject emitter;
    public GameObject fireball;
    public int fbFireCt;
    public int fbFireLimit;
    public bool fbCanFire;
    public float fireTime;

    public bool grabEnabled;
    public Transform GrabBeam;

    public bool canArcTp;

    public AudioClip fireCastSound;

    List<GameObject> arcPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        attachPoint = Instantiate<GameObject>(new GameObject(), this.transform).GetComponent<Transform>();
        canSnapRotate = true;
        fbCanFire = true;
        canArcTp = true;
        grabEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //gen arc
        Vector3 arcVelocity = laser.forward * arcSpeed;
        Vector3 arcPos = laser.position;
        Vector3 footPos = head.position;
        footPos.y -= head.localPosition.y;

        //Left Hand
        if (!rightHand)
        {

            float triggerValue = Input.GetAxis("Oculus_GearVR_LGripTrigger");
            if (triggerValue < 0.4f && attachedRigidBody != null)
            {
                attachedRigidBody = null;
                this.GetComponent<MeshRenderer>().enabled = true;
            }
            float litrigger = Input.GetAxis("Oculus_GearVR_LIndexTrigger");
            if(litrigger <= 0.5f)
            {
                canArcTp = true;
            }

            float distance = 0;
            foreach (GameObject p in arcPoints)
            {
                GameObject.Destroy(p);
            }
            arcPoints.Clear();
            while (distance < 10)
            {
                Vector3 delta_p = arcVelocity * 0.01f;

                RaycastHit[] hits = Physics.RaycastAll(arcPos, arcVelocity.normalized, delta_p.magnitude);
                bool arcHit = false;
                Vector3 arcHitPoint = Vector3.zero;
                /*
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    GameObject gameObj2 = GameObject.Instantiate<GameObject>(arcPointPrefab, hit.point, Quaternion.identity);
                    gameObj2.transform.forward = arcVelocity.normalized;
                    gameObj2.transform.localScale = new Vector3(1.0f, 1.0f, 0.01f);
                    arcPoints.Add(gameObj2);
                    arcHit = true;
                    arcHitPoint = hit.point;
                    break;
                }
                */
                bool hitTags = false;
                if(hits.Length > 0)
                {
                    RaycastHit hit = hits[0];
                    GameObject gameObj2 = GameObject.Instantiate<GameObject>(arcPointPrefab, hit.point, Quaternion.identity);
                    gameObj2.transform.forward = arcVelocity.normalized;
                    gameObj2.transform.localScale = new Vector3(1.0f, 1.0f, 0.01f);
                    arcPoints.Add(gameObj2);
                    arcHit = true;
                    arcHitPoint = hit.point;
                    if (hit.transform.tag != "Avatar" && hit.transform.tag != "Rig" && hit.transform.tag != "Projectile" && hit.transform.tag != "FireballProjectile")
                    {
                        hitTags = true;
                    }
                }
                if (arcHit && hitTags)
                {
                    Vector3 targetPoint = arcHitPoint;
                    Vector3 offset = targetPoint - footPos;
                    //offset = offset.normalized * (arcSpeed * 0.1f);

                    //bool triggerValue = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue;
                    //float trigger = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger, myHand);

                    if (litrigger > 0.5f && canArcTp)
                    {
                        offset = offset + new Vector3(0.0f, 1.0f, 0.0f);
                        trackingSpace.Translate(offset, Space.World);
                        
                        float tsPosY = trackingSpace.position.y;
                        if(tsPosY < 1.0f)
                        {
                            Vector3 offset2 = new Vector3(0.0f, 0.5f - tsPosY, 0.0f);
                            trackingSpace.Translate(offset2, Space.World);
                        }
                        if(tsPosY > 10.0f)
                        {
                            Vector3 offset3 = new Vector3(0.0f, -(tsPosY - 10.0f), 0.0f);
                            trackingSpace.Translate(offset3, Space.World);
                        }
                        
                        canArcTp = false;
                        
                    }
                    break;
                }

                arcPos += delta_p;
                arcVelocity += new Vector3(0, -9.8f, 0) * .01f;
                distance += delta_p.magnitude;

                GameObject gameObj = GameObject.Instantiate(arcPointPrefab, arcPos, Quaternion.identity);
                gameObj.transform.forward = arcVelocity.normalized;
                gameObj.transform.localScale = new Vector3(1, 1, delta_p.magnitude);
                arcPoints.Add(gameObj);
            }

            if (trackingSpace.transform.position.y == 0.5f)
            {
                float up = Input.GetAxis("Oculus_GearVR_LThumbstickY");
                float right = Input.GetAxis("Oculus_GearVR_LThumbstickX");

                Vector3 headForwardVector = head.forward;
                headForwardVector.y = 0;
                headForwardVector.Normalize();

                Vector3 headRightVector = head.right;
                headRightVector.y = 0;
                headRightVector.Normalize();

                Vector3 direction = headForwardVector * up + headRightVector * right;
                trackingSpace.transform.Translate(direction * speed * Time.deltaTime, Space.World);
            }
        }

        //Right Hand
        else
        {
            float rGripTrigger = Input.GetAxis("Oculus_GearVR_RGripTrigger");
            float right = Input.GetAxis("Oculus_GearVR_RThumbstickX");
            float up = Input.GetAxis("Oculus_GearVR_RThumbstickY");
            float bBtn = Input.GetAxis("Fire2");
            
            if (bBtn > 0.5f)
            {
                grabEnabled = grabEnabled ? false : true;
                GrabBeam.gameObject.SetActive(grabEnabled);
            }
            
            float rightMag = Mathf.Abs(right);
            float snapRotateDelta = snapRotateDeltaSmall;
            if (rightMag > 0.9f && canSnapRotate)
            {
                if (rGripTrigger > 0.5f && !grabEnabled)
                {
                    snapRotateDelta = 90;
                }
                trackingSpace.transform.RotateAround(footPos, Vector3.up, snapRotateDelta * Mathf.Sign(right));
                canSnapRotate = false;
            }
            else
            {
                if (rightMag < 0.2f)
                {
                    canSnapRotate = true;
                }
                
                trackingSpace.transform.RotateAround(footPos, Vector3.up, rotateSpeed * right * Time.deltaTime);
                //trackingSpace.transform.RotateAround(footPos, Vector3.forward, rotateSpeed * -up * Time.deltaTime);
                
            }

            float rIndexTrigger = Input.GetAxis("Oculus_GearVR_RIndexTrigger");
            if ((rIndexTrigger > 0.5f && fbCanFire) || Input.GetMouseButtonDown(0))
            {
                PhotonNetwork.Instantiate(fireball.name, emitter.transform.position, emitter.transform.rotation);
                fireTime = 0.0f;
                fbFireCt--;
                if (fbFireCt == 0)
                    AudioSource.PlayClipAtPoint(fireCastSound, emitter.transform.position);
            }

            if (fbFireCt <= 0)
            {
                fbCanFire = false;
            }
            fireTime += Time.deltaTime;
            if (fireTime >= 1.0f) {
                fbFireCt = fbFireLimit;
                fbCanFire = true;
            }
        }

    }

    private void FixedUpdate()
    {
        if (attachedRigidBody != null)
        {
            Vector3 difference = attachedRigidBody.position - attachPoint.position;
            attachedRigidBody.velocity = -difference / Time.fixedDeltaTime;

            Quaternion rotationDifference = attachedRigidBody.rotation * Quaternion.Inverse(attachPoint.rotation);
            float angle;
            Vector3 axis;
            rotationDifference.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = -Mathf.Deg2Rad * angle / Time.fixedDeltaTime * axis;
            attachedRigidBody.angularVelocity = angularVelocity;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (!rightHand)
        {
            Rigidbody otherRb = other.attachedRigidbody;
            if (otherRb == null) return;
            float triggerValue = Input.GetAxis("Oculus_GearVR_LGripTrigger");
            if (triggerValue > 0.5f && attachedRigidBody == null)
            {
                attachedRigidBody = otherRb;
                attachedRigidBody.maxAngularVelocity = Mathf.Infinity;
                attachPoint.position = attachedRigidBody.position;
                attachPoint.rotation = attachedRigidBody.rotation;
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (triggerValue < 0.4f && attachedRigidBody != null)
            {
                attachedRigidBody = null;
                this.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
    
}
