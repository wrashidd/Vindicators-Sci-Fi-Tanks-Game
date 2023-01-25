using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Bullet_Controller : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _bulletExplosionPFX;

    private float lifeTime = 6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleShoot();
    }


    private void HandleShoot(){
        _rb.velocity = transform.forward * _moveSpeed;
        //Bullet Life Time if not hit anything
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0 ){
        Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other){

        if(other.tag == "Player"){
            //Igonore Player Collisions

        }
        else{
            Destroy(gameObject);
            Instantiate(_bulletExplosionPFX, transform.position + (transform.forward * (-_moveSpeed * 1.9f *Time.deltaTime)), transform.rotation);
        }
        
    }
}
