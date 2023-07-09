using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class PlayerMovement: NetworkBehaviour
{
        
        [Header("Camera")]
        public Camera camera;
        
        [Header("Movement")]
        [SerializeField] private CharacterController _controller;
        [SerializeField] private float PlayerSpeed;
        
        [Header("Bullets")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private float _shootCooldown;
        [SerializeField] bool _canShoot = true;
        
        [Header("Animation")]
        [SerializeField] private Animator _animator; 
        
        [Header("Jump")]
        [SerializeField] public float _jumpForce = 5f;
        [SerializeField] public float _gravity = -9.81f;

        private Vector3 velocity;
        private bool _jumpPressed;
        private bool _isGrounded;

        private void Update()
        {
                if (Input.GetButtonDown("Jump"))
                {
                        _jumpPressed = true;
                }
        }

        //In Fusion, gameplay code that updates every tick such as movement should  run in
        //FixedUpdateNetwork
        public override void FixedUpdateNetwork()
        {
                //HasStateAuthority can be used to check whether the client controls an object
                if (HasStateAuthority == false)
                {
                        return;
                }

                
                        
                        
                        if (_controller.isGrounded)
                        {
                                velocity = new Vector3(0, -1, 0);
                        }

                       // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) *
                        //Runner.DeltaTime * PlayerSpeed;
                        
                        var cameraRotationY = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
                        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;

                        velocity.y += _gravity * Runner.DeltaTime;
                        
                        if (_jumpPressed && _controller.isGrounded)
                        {
                                velocity.y += _jumpForce;
                        }
                        
                        _controller.Move(move  + velocity * Runner.DeltaTime);

                        if (move != Vector3.zero)
                        {
                                gameObject.transform.forward = move;
                                _animator.SetBool("IsMoving", true);
                        }
                        else if (move == Vector3.zero)
                        {
                                _animator.SetBool("IsMoving", false);
                        }
                       
                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                                Shoot();
                        }

                        _jumpPressed = false;
              
        }

        //player object gets spawned find and set up the camera if it is the local player.
        public override void Spawned()
        {
                if (HasStateAuthority)
                {
                        _controller = gameObject.GetComponent<CharacterController>();
                        
                        camera = Camera.main;
                        camera.GetComponent<ThirdPersonCamera>()._target = gameObject.transform;
                        //camera.GetComponent<FirstPersonCamera>()._targer = gameObject.transform;
                }
        }
        #region Shoot

                void Shoot()
                {
                        if (_canShoot) {_animator.SetBool("IsMoving", false); _animator.SetBool("IsShooting", true);StartCoroutine(Shoot_Cooldown()); }
                }

                IEnumerator Shoot_Cooldown()
                {
                        
                        _canShoot = false;
                        Runner.Spawn(_bulletPrefab, _bulletSpawnPoint.position, transform.rotation);

                        yield return new WaitForSeconds(_shootCooldown);

                        _canShoot = true;
                        _animator.SetBool("IsShooting", false);

                }

        #endregion

        #region Jump

                void Jump()
                {
                        if (_controller.isGrounded)
                        {
                                velocity.y += Mathf.Sqrt(_jumpForce * -3.0f * _gravity);
                        }
                }
        #endregion
}
