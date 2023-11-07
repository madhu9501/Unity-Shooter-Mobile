using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace TDM
{
    [RequireComponent (
        typeof(CharacterController), 
        typeof(Animator)
    )]

    class PlayerController : MonoBehaviour
    {
        [Header ("Player movement")]
        public float MoveSpeed = 5f;
        public float SpritnSpeed = 1f;
        [SerializeField]
        float _smoothTime = 0.1f;
        [SerializeField]
        float _turnVel;
        Vector3 _dir;
        public Vector3 Dir{ get { return _dir;}}

        [Header("Player gravity")]
        [SerializeField]
        LayerMask _groundLayerMask;
        [SerializeField]
        Transform _groundCheckSphereTransform;
        Vector3 _verticalVel;
        float _gravity = -9.81f;
        float _sphereRadius = 0.4f;
        bool _isGrounded;

        [Header("Player jump")]
        [SerializeField]
        float _jumpHight = 2f;
        
        [Header("Player stats")]
        [SerializeField]
        int _health = 100;

        // Components
        [Header ("Camer")]
        [SerializeField]
        Transform _playerCamera;
        CharacterController _characterController;
        Objects _objects;

        public bool MobileInputs;
        public FixedJoystick fixedJoystick;

        // Animation
        private Animator _animator;
        // private readonly int _moveSpeedHash = Animator.StringToHash("MoveSpeed");


        //Singleton
        private static PlayerController _playerInstance;
        public static PlayerController PlayerInstance{get{return _playerInstance; }}


        void Awake()
        {
            _playerInstance = this;
            _characterController = GetComponent<CharacterController>();
            _objects = GetComponent<Objects>();
            _animator = GetComponent<Animator>();

        }

        void Start(){
            // Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            ApplyGravity();
            PlayerMove();
            PlayerJump();
        }

        void PlayerMove()
        {
            if(MobileInputs == true)
            {
                float verMove = fixedJoystick.Vertical;
                float horMove = fixedJoystick.Horizontal;
                Vector3 dir = new Vector3(horMove, 0f, verMove).normalized;

                if(dir.magnitude >= 0.1f)
                {
                    // Camera angle + the keyboard angle
                    float tgtAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _playerCamera.eulerAngles.y;
                    // _turnVel is automatically calaculated
                    float smoothTgtAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tgtAngle, ref _turnVel, _smoothTime);
                    // Face the direction of aim
                    // if(Input.GetMouseButton(1))
                    // {
                    //     transform.rotation = Quaternion.Euler(0f, _playerCamera.localRotation.eulerAngles.y, 0f);
                    // }else
                    // {
                    transform.rotation = Quaternion.Euler(0f, smoothTgtAngle, 0f);
                    // }

                    if(CrossPlatformInputManager.GetButton("Sprint"))
                    {
                        SpritnSpeed = 2f;
                    }else
                    {
                        SpritnSpeed = 1f;
                    }
                    // change the direction/angle of fwd vector
                    Vector3 moveFwdCamDir = Quaternion.Euler(0f, tgtAngle, 0f) * Vector3.forward; 
                    _characterController.Move(moveFwdCamDir.normalized * MoveSpeed * SpritnSpeed * Time.deltaTime);
                }
            }
            else
            {
                float verMove = Input.GetAxisRaw("Vertical");
                float horMove = Input.GetAxisRaw("Horizontal");
                Vector3 dir = new Vector3(horMove, 0f, verMove).normalized;

                if(dir.magnitude >= 0.1f)
                {
                    // Camera angle + the keyboard angle
                    float tgtAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + _playerCamera.eulerAngles.y;
                    // _turnVel is automatically calaculated
                    float smoothTgtAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tgtAngle, ref _turnVel, _smoothTime);
                    // Face the direction of aim
                    // if(Input.GetMouseButton(1))
                    // {
                    //     transform.rotation = Quaternion.Euler(0f, _playerCamera.localRotation.eulerAngles.y, 0f);
                    // }else
                    // {
                    transform.rotation = Quaternion.Euler(0f, smoothTgtAngle, 0f);
                    // }

                    if(Input.GetButton("Sprint"))
                    {
                        SpritnSpeed = 2f;
                    }else
                    {
                        SpritnSpeed = 1f;
                    }
                    // change the direction/angle of fwd vector
                    Vector3 moveFwdCamDir = Quaternion.Euler(0f, tgtAngle, 0f) * Vector3.forward; 
                    _characterController.Move(moveFwdCamDir.normalized * MoveSpeed * SpritnSpeed * Time.deltaTime);
                

                }
            }
        }

        void ApplyGravity()
        {
            _isGrounded = Physics.CheckSphere(_groundCheckSphereTransform.position, _sphereRadius, _groundLayerMask);
            if(_isGrounded && _verticalVel.y <= 0)
            {
                _verticalVel.y = -0.2f;
            }
            _verticalVel.y += _gravity * Time.deltaTime;
            _characterController.Move(_verticalVel * Time.deltaTime);

        }

        void PlayerJump()
        {
            
            if(Input.GetButtonDown("Jump") && _isGrounded)
            {
                // sqrt(2gh);  -2 to compensate for negative gravity
                _verticalVel.y = Mathf.Sqrt(-2f * _gravity * _jumpHight );
            }
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            // Debug.Log(_health);

            if(_health <= 0)
            {
                PLayerDeath();

            }
        }

        private void PLayerDeath()
        {
            PlayerRespawn();
        }

        private void PlayerRespawn()
        {
            //
        }



    }
}

