using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vindicator {

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Tank_Inputs))]
public class Tank_Controller : MonoBehaviour
{
    private Rigidbody rb;
    private Tank_Inputs input;

    [Header("Movement Proporties")]
    public float tankSpeed = 15f;
    public float tankRotationSpeed = 50f;

    [Header("Target Proporties")]
    public  Transform targetTransform;


    [Header("Head Properties")]
    public Transform headTransform;
    public Transform cannonTransform;

    public Transform headDefaultDirection;
    private bool _headTurned = false;
   
    

    [SerializeField] private float headLagSpeed = 1.2f;

    private Vector3 finalHeadLookDir;
    private Vector3 finalCannonLookDir;
    private bool safeZoneOn;
    private bool rightClick = false;
   

    // public Transform headRot;


    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
       input = GetComponent<Tank_Inputs>(); 
      
    }

void Update(){
    HandleRightClick();
}
// Update is called once per frame
    void FixedUpdate(){
        if(rb && input){
            HandleMovement();
            HandleHead();
            HandleTarget();
            if(rightClick==false){
                HandleHeadDefDir();
            }
        }
    }
 



    protected virtual void HandleMovement(){
        //Movies tank forward
        Vector3 wantedPosition = transform.position + (transform.forward * input.ForwardInput * tankSpeed * Time.deltaTime);
        rb.MovePosition(wantedPosition);

        //Rotates the tank
        Quaternion wantedRotation = transform.rotation * Quaternion.Euler(Vector3.up * (tankRotationSpeed * input.RotationInput * Time.deltaTime));
        rb.MoveRotation(wantedRotation);

    }

    // protected virtual void HandleHead(){
    //    if(headTransform){
    //     Vector3 headLookDir = input.TargetPosition - headTransform.position;
    //     headLookDir.y = 0;
    //     finalHeadLookDir = Vector3.Lerp(finalHeadLookDir, headLookDir, Time.deltaTime * headLagSpeed);
    //     headTransform.rotation = Quaternion.LookRotation(finalHeadLookDir);
    //     //For Tank_Inputs Script that needs head rot pos
    //     // headRot.rotation= headTransform.rotation;
    //    } 
    // }

      protected virtual void HandleHead(){
       if(headTransform && cannonTransform && rightClick){
        Vector3 headLookDir = input.TargetPosition - headTransform.position;
        headLookDir.y = 0;
        Vector3 cannonLookDir = input.TargetPosition - cannonTransform.position;
        if(cannonLookDir.y >=10f || cannonLookDir.y <= -2.9f){
            // cannonLookDir = new Vector3(transform.position.x, 0, transform.position.z);
            cannonLookDir.y =0;
        }
        else{
            //Shoot higher than targets position
            cannonLookDir.y = cannonLookDir.y + 0.5f;
        }
        finalHeadLookDir = Vector3.Lerp(finalHeadLookDir, headLookDir, Time.deltaTime * headLagSpeed);
        finalCannonLookDir = Vector3.Lerp(finalCannonLookDir, cannonLookDir, Time.deltaTime *headLagSpeed);
        headTransform.rotation = Quaternion.LookRotation(finalHeadLookDir);
        cannonTransform.rotation = Quaternion.LookRotation(finalCannonLookDir);
       } 
  
   
    }

    
    private void HandleHeadDefDir(){
          // Face Tank Head to default position 
         Vector3 headLookDir = headDefaultDirection.position - headTransform.position;
        headLookDir.y = 0;
        Vector3 cannonLookDir = headDefaultDirection.position  - cannonTransform.position;
        cannonLookDir.y=0;
        finalHeadLookDir = Vector3.Lerp(finalHeadLookDir, headLookDir, Time.deltaTime * headLagSpeed);
        finalCannonLookDir = Vector3.Lerp(finalCannonLookDir, cannonLookDir, Time.deltaTime *headLagSpeed);
        headTransform.rotation = Quaternion.LookRotation(finalHeadLookDir);
        cannonTransform.rotation= Quaternion.LookRotation(finalCannonLookDir);

        // Debug.DrawRay(headTransform.position, headDefaultDirection.position, Color.green);
        

    }

    




    protected virtual void HandleTarget(){
        if(targetTransform && rightClick){
           targetTransform.position = input.TargetPosition;
        }

    }



    private void HandleRightClick(){
           if(Input.GetMouseButtonDown(1)){
            rightClick = true;
        }
        if (Input.GetMouseButtonUp(1)){
            rightClick= false;  
            //  headTransform.localRotation= Quaternion.Euler(0f,0f,0f); 
            //  cannonTransform.localRotation= Quaternion.Euler(0f,0f,0f); 

        }
    }


}

}


