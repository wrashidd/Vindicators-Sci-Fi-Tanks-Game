using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{

   public bool b_safeZone =false;
    private bool SafeZoneTrigger;
        // Start is called before the first frame update
    void Start()
    {
        
    }
  private void OnTriggerEnter(Collider other){
    if(other.tag == "SafeZone"){
        
       b_safeZone = true;
    }
  }
   private void OnTriggerExit(Collider other){
    if(other.tag == "SafeZone"){
        
       b_safeZone = false;
    }
  }
}
