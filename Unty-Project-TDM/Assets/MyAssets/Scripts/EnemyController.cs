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
        typeof(EnemyController), 
        typeof(Animator),
        typeof(NavMeshAgent)
    )]

    [RequireComponent (
        typeof(CapsuleCollider)
    )]

    public class EnemyController : MonoBehaviour
    {
        //Empty Gameobject in enemy 
        public GameObject shootOrigin;
        public LayerMask PlayerMask;
        public LayerMask AllyMask;
        NavMeshAgent _navMeshAgent;
        Transform _player;
        public Transform SpawnPoint;
        public Transform EnemyCharcters;

        // [SerializeField]
        // float _enemySpeed = 3f;

        [Header ("Enemy weapon")]
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
        bool _isPlayer;

        [Header ("Enemy stats")]
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
            _player = GameObject.Find("Player").GetComponent<Transform>();
        }

        void Update()
        {
            _inVisionRange = Physics.CheckSphere(transform.position, _visionRange, PlayerMask); 
            _inShootingRange = Physics.CheckSphere(transform.position, _shootingRange, PlayerMask); 

            if(_inVisionRange && !_inShootingRange) ChaseTarget();
            if(_inVisionRange && _inShootingRange) ShootTarget();

        }

        private void ChaseTarget()
        {
            if(_navMeshAgent.SetDestination(_player.position))
            {
                // Animation
            }
        }

        private void ShootTarget()
        {
            _navMeshAgent.SetDestination(transform.position);
            transform.LookAt(_player);
            if(!_didShoot){
                if(Physics.Raycast(shootOrigin.transform.position, shootOrigin.transform.forward, out RaycastHit hitInfo, _shootingRange, ((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ally")))))
                    if (hitInfo.transform.gameObject.layer == (1 << LayerMask.NameToLayer("Player")) )
                    {
                        if (hitInfo.transform.GetComponent<PlayerController>())
                        {
                            PlayerController.PlayerInstance.TakeDamage(_damage);
                            Debug.Log("Hit");
                        }
                        PlayerController.PlayerInstance.TakeDamage(_damage);
                    }
                    else if (hitInfo.transform.gameObject.layer == (1 << LayerMask.NameToLayer("Ally")) )
                    {
                        AllyController allyController = hitInfo.transform.GetComponent<AllyController>();
                        if (allyController!= null)
                        {
                            PlayerController.PlayerInstance.TakeDamage(_damage);
                        }
                    }
                {
                    PlayerController.PlayerInstance.TakeDamage(_damage);
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
                EnemyDeath();

            }
        }

        private void EnemyDeath()
        {
            StartCoroutine(nameof(EnemyRespawn));
        }

        private IEnumerator EnemyRespawn()
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

            //EnemyCharcters.
            transform.position = SpawnPoint.transform.position;
            ChaseTarget();


        }





    }
}