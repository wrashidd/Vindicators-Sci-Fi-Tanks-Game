using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vindicator
{
    public class Tank_Inputs : MonoBehaviour
    {
        [Header("Input Proporties")]
        public Camera mCamera;
        private bool cameraZoomIn = false;

        [SerializeField]
        private GameObject bullet1Prefab;

        [SerializeField]
        private GameObject tankCannon;

        //Tracks
        public GameObject rightTrack;
        public GameObject leftTrack;

        private Animator leftTrackAnim;
        private Animator rightTrackAnim;
        private Animator cannonShoot;

        // private bool trackIdle= true;

        //Camera View Switch



        //Get target and its position
        [SerializeField]
        private GameObject targetTransform;
        private Renderer targetRenderer;

        private Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            get { return targetPosition; }
        }

        private Vector3 targetNormal;
        public Vector3 TargetNormal
        {
            get { return targetNormal; }
        }

        //Get Default Head Direction
        [SerializeField]
        private Transform firePoint;

        private float forwardInput;
        public float ForwardInput
        {
            get { return forwardInput; }
        }

        private float rotationInput;
        public float RotationInput
        {
            get { return rotationInput; }
        }

        void Start()
        {
            rightTrackAnim = rightTrack.gameObject.GetComponent<Animator>();
            leftTrackAnim = leftTrack.gameObject.GetComponent<Animator>();
            cannonShoot = tankCannon.GetComponent<Animator>();
            targetRenderer = targetTransform.GetComponent<Transform>().GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (mCamera)
            {
                HandleInputs();
                HandleShoot();
                HandleCamView();
            }
        }

        protected virtual void HandleInputs()
        {
            Ray screenRay = mCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(screenRay, out hit))
            {
                targetPosition = hit.point;
                targetNormal = hit.normal;
                targetTransform.transform.rotation = Quaternion.LookRotation(
                    targetNormal,
                    Vector3.up
                );
                if (hit.transform.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy:Setting Color Green");
                    targetRenderer.material.color = new Color(0.4901f, 1f, 0.1764f, 1f);
                    targetRenderer.material.SetColor(
                        "_EmissionColor",
                        new Color(0.4901f, 1f, 0.1764f, 0.5f)
                    );
                }
                else
                {
                    targetRenderer.material.color = Color.white;
                    targetRenderer.material.SetColor("_EmissionColor", new Color(0f, 0f, 0f, 0f));
                }
            }
            forwardInput = Input.GetAxis("Vertical");
            rotationInput = Input.GetAxis("Horizontal");

            //Track Animation
            if (rightTrackAnim && leftTrackAnim != null)
            {
                if (Input.GetButtonDown("Drive"))
                {
                    Debug.Log("Drive is on");
                    rightTrackAnim.SetTrigger("Drive");
                    leftTrackAnim.SetTrigger("Drive");
                }
                if (Input.GetButtonUp("Drive"))
                {
                    rightTrackAnim.SetTrigger("Idle");
                    leftTrackAnim.SetTrigger("Idle");
                    Debug.Log("Key is Up going Idle");
                }

                if (Input.GetButtonDown("Reverse"))
                {
                    rightTrackAnim.SetTrigger("Reverse");
                    leftTrackAnim.SetTrigger("Reverse");
                }

                if (Input.GetButtonUp("Reverse"))
                {
                    rightTrackAnim.SetTrigger("Idle");
                    leftTrackAnim.SetTrigger("Idle");
                }
            }
        }

        protected virtual void HandleShoot()
        {
            //when left mouse button pressed
            if (Input.GetMouseButtonDown(0))
            {
                // //play cannon move animation
                cannonShoot.SetTrigger("Shoot");
                //create bullet
                Instantiate(bullet1Prefab, firePoint.position, firePoint.rotation);
            }

            if (Input.GetMouseButtonUp(0))
            {
                cannonShoot.SetTrigger("Normal");
            }
        }

        //Camera Zoom In
        private void HandleCamView()
        {
            if (cameraZoomIn == false)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    mCamera.orthographicSize = 14;
                    cameraZoomIn = true;
                }
                else
                {
                    if (Input.GetKey(KeyCode.F))
                    {
                        mCamera.orthographicSize = 20;
                        cameraZoomIn = false;
                    }
                }
            }
        }
    }
}
