using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDM;
using UnityStandardAssets.CrossPlatformInput;

namespace TDM
{
    // public enum WeaponType{
    //     SUBMACHINE,
    //     MACHINE,
    //     PISTOL,
    //     SHOTGUN,
    //     SNIPER
    // }

    public class Weapon : MonoBehaviour
    {
        public Camera RifleCam;
        public ParticleSystem MuzzleFlash;
        public GameObject ImpactPrefab;
        public GameObject BloodPrefab;
        private CameraController camView;   


        [Header("Rifle")]
        [SerializeField]
        int _damage;
        [SerializeField]
        float _range;
        [SerializeField]
        float _fireRate = 15f;
        float _fireCharge =0f;

        [Header("Ammunition")]
        [SerializeField]
        int _maxAmmo = 25;
        int _currentAmmo;
        [SerializeField]
        int _magCount = 10;
        [SerializeField]
        float _reloadTime = 1.5f;
        bool _isReloading;


        void Start()
        {
            _currentAmmo = _maxAmmo;
            camView =  FindObjectOfType<CameraController>();
        }

        void Update()
        {
            if (_isReloading) { return; }

            if(_currentAmmo <= 0) 
            { 
                StartCoroutine(ReloadAmmo()); 
                return;
            }


            // PISTOL, SHOTGUN, SNIPER
            // if(Input.GetMouseButtonDown(0))
            // {
            //     FireGun();
            // }
            
            // MACHINE, SUBMACHINE
            if(CrossPlatformInputManager.GetButton("Shoot") && Time.time >= _fireCharge)
            {
                _fireCharge = Time.time + 1f/_fireRate;
                FireGun();
                // Animations
            }


            // Camera control
            if(CrossPlatformInputManager.GetButton("Shoot") && (PlayerController.PlayerInstance.Dir.magnitude >= 0.1f) )
            {
                camView.AimView();
                // Animations
            }else if(CrossPlatformInputManager.GetButton("Aim")) //add if statement for aim and shoot to set animations
            {
                camView.AimView();
                // Animations
            }else
            {
                camView.ThirdPersonView();
                // Animations
            }
        }

        IEnumerator ReloadAmmo()
        {
            PlayerController.PlayerInstance.MoveSpeed = PlayerController.PlayerInstance.MoveSpeed/2;
            PlayerController.PlayerInstance.SpritnSpeed = 1f;
            _isReloading = true;
            yield return new WaitForSeconds(_reloadTime);
            _currentAmmo = _maxAmmo;
            PlayerController.PlayerInstance.MoveSpeed = PlayerController.PlayerInstance.MoveSpeed*2;
            PlayerController.PlayerInstance.SpritnSpeed = 2f;
            _isReloading = false;

        }

        void FireGun()
        {
            if(PlayerController.PlayerInstance.MobileInputs == true)
            {
                if(_magCount <= 0 ){ return; }
                _currentAmmo--;
                if(_currentAmmo == 0 ){ _magCount--; }

                MuzzleFlash.Play(); 
                if(Physics.Raycast(RifleCam.transform.position, RifleCam.transform.forward, out RaycastHit hit, _range))
                {
                    Objects objects = hit.collider.gameObject.GetComponent<Objects>();
                    EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();

                    if(objects != null)
                    {
                        objects.TakeDamage(_damage);
                        GameObject impactObj = Instantiate(ImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impactObj, 1f);
                    }
                    else if(enemyController != null)
                    {
                        enemyController.TakeDamage(_damage);
                        GameObject bloodObj = Instantiate(BloodPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(bloodObj, 1f);
                    } 
                }
            }
            else
            {
                if(_magCount <= 0 ){ return; }
                _currentAmmo--;
                if(_currentAmmo == 0 ){ _magCount--; }

                MuzzleFlash.Play(); 
                if(Physics.Raycast(RifleCam.transform.position, RifleCam.transform.forward, out RaycastHit hit, _range))
                {
                    Objects objects = hit.collider.gameObject.GetComponent<Objects>();
                    EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();

                    if(objects != null)
                    {
                        objects.TakeDamage(_damage);
                        GameObject impactObj = Instantiate(ImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(impactObj, 1f);
                    }
                    else if(enemyController != null)
                    {
                        enemyController.TakeDamage(_damage);
                        GameObject bloodObj = Instantiate(BloodPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(bloodObj, 1f);
                    } 
                }
            }
        }

        
    }
}