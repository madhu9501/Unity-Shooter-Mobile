using System;
using System.Collections;
using System.Collections.Generic;
using TDM;
using UnityEngine;
using UnityEngine.AI;

// create a spawn enemy script
namespace TDM
{  

    [RequireComponent (
        typeof(AllyController), 
        typeof(Animator),
        typeof(NavMeshAgent)
    )]

    [RequireComponent (
        typeof(CapsuleCollider)
    )]

    public class AllyController : MonoBehaviour
    {
        //Empty Gameobject in enemy 
        public GameObject shootOrigin;
        public LayerMask EnemyMask;
        NavMeshAgent _navMeshAgent;
        public Transform _enemy;
        public Transform SpawnPoint;
        public Transform AllyCharcters;

        // [SerializeField]
        // float _enemySpeed = 3f;

        [Header ("Ally weapon")]
        [SerializeField]
        int _damage = 5;
        [SerializeField]
        float _visionRange = 15f;
        [SerializeField]
        float _shootingRange = 8f;
        [SerializeField]
        float _timeBewteenShoot = 1f;
        bool _inVisionRange;
        bool _inShootingRange;
        bool _didShoot;

        [Header ("Ally stats")]
        [SerializeField]
        int _maxHitPoint = 100; 
        int _currentHitPoint; 


        void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _currentHitPoint = _maxHitPoint;
            
        }

        void Start()
        {
            // var _enemy = GameObject.FindGameObjectsWithTag("Enemy"); //.GetComponent<Transform>();
            // in a loop, comapre the dist between this tansform and target transform. save thm in an array. chose the arry index with least distvalue and set it as target to be chased and shot
        }

        void Update()
        {
            _inVisionRange = Physics.CheckSphere(transform.position, _visionRange, EnemyMask); 
            _inShootingRange = Physics.CheckSphere(transform.position, _shootingRange, EnemyMask); 

            if(_inVisionRange && !_inShootingRange) ChaseTarget();
            if(_inVisionRange && _inShootingRange) ShootTarget();

        }

        private void ChaseTarget()
        {
            if(_navMeshAgent.SetDestination(_enemy.position))
            {
                // Animation
            }
        }

        private void ShootTarget()
        {
            _navMeshAgent.SetDestination(transform.position);
            transform.LookAt(_enemy);
            if(!_didShoot){
                if(Physics.Raycast(shootOrigin.transform.position, shootOrigin.transform.forward, out RaycastHit hitInfo, _shootingRange,  EnemyMask))
                {
                    EnemyController enemyController = hitInfo.collider.gameObject.GetComponent<EnemyController>();
                    if(enemyController != null)
                    {
                        enemyController.TakeDamage(_damage);
                        // GameObject bloodObj = Instantiate(BloodPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        // Destroy(bloodObj, 1f);
                    } 
                }
            }

            _didShoot = true;
            Invoke(nameof(ActivateShooting), _timeBewteenShoot);


        }

        private void ActivateShooting()
        {
            _didShoot = false;
        }

        public void TakeDamage(int damage)
        {
            _currentHitPoint -= damage;
            // Debug.Log("Enemy: " + _currentHitPoint);

            if(_currentHitPoint <= 0)
            {
                AllyDeath();

            }
        }

        private void AllyDeath()
        {
            StartCoroutine(nameof(AllyRespawn));
        }

        private IEnumerator AllyRespawn()
        {
            _navMeshAgent.SetDestination(transform.position);
            _visionRange = 0f;
            _shootingRange = 0f;
            _timeBewteenShoot = 0f;
            _inVisionRange = false;
            _inShootingRange = false;

            yield return new WaitForSeconds(5f);

            _currentHitPoint = _maxHitPoint;
             _visionRange = 15f;
            _shootingRange = 8f;
            _timeBewteenShoot = 1f;
            _inVisionRange = true;

            AllyCharcters.transform.position = SpawnPoint.transform.position;
            ChaseTarget();


        }





    }
}