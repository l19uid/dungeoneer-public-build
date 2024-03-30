using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Dungeoneer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        public float defaultSpeed = 7500f;
        public float speed = 5f;
        public float walkLimit = 5f;
        public float rollSpeed = 15f;
        public bool canMove = true;
        private bool _isFrozen = false;
        public float _rollTime;
        public float staminaMax;
        public float stamina;
        public int _rollCost = 25;
        
        private Vector2 _input;
        private int stepCount;
        private Health _playerHealth;
        private bool _isSprinting = false;
        private bool _isRolling = false;
        public bool isCharging = false;
        private bool _openUI = false;
        
        private PlayerAttack _playerAttack;
        private UIManager _uiManager;
        private Animator _animator;
        private Rigidbody2D _rb;
        public Animator helmetAnimator;
        public Animator chestAnimator;
        public Animator legsAnimator;
        public Animator bootsAnimator;
        private Inventory _inventory;
        private Player _player;

        private Vector2 _inputMovement;
        private Vector2 _knockbackMovement;
        
        private readonly int _rollHash = Animator.StringToHash("player_roll");
        private readonly int _runHash = Animator.StringToHash("player_running");
        private readonly int _idleHash = Animator.StringToHash("player_idle");
        
        private int _helmetRollHash = Animator.StringToHash("player_roll");
        private int _helmetRunHash = Animator.StringToHash("player_running");
        private int _helmetIdleHash = Animator.StringToHash("player_idle");
        
        private int _chestplateRollHash = Animator.StringToHash("player_roll");
        private int _chestplateRunHash = Animator.StringToHash("player_running");
        private int _chestplateIdleHash = Animator.StringToHash("player_idle");
        
        private int _leggingsRollHash = Animator.StringToHash("player_roll");
        private int _leggingsRunHash = Animator.StringToHash("player_running");
        private int _leggingsIdleHash = Animator.StringToHash("player_idle");
        
        private int _bootsRollHash = Animator.StringToHash("player_roll");
        private int _bootsRunHash = Animator.StringToHash("player_running");
        private int _bootsIdleHash = Animator.StringToHash("player_idle");
        
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inventory = GetComponent<Inventory>();
            _playerAttack = GetComponent<PlayerAttack>();
            _playerHealth = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();
            _uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
            _player = GetComponent<Player>();
            UpdateStats();
            Invoke(nameof(UpdateArmorHashes), 0.1f);
        }

        // Update is called once per frame
        void Update()
        {
            HandleStamina();
            if (_openUI)
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            HandleInput();

            if (canMove)
                Movement();
            else if (!canMove && !_isRolling)
                _rb.velocity = Vector2.zero;

            ManageAnims();
        }

        private void HandleInput()
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");

            Debug.DrawRay(transform.position, _input.normalized * 2f, Color.red);
            if (Input.GetKeyDown(KeyCode.LeftShift) && stamina > 20f && !_isRolling && !isCharging && canMove)
                StartCoroutine(Roll());
        }

        private void ManageAnims()
        {
            if (_isRolling)
                return;

            if (_input.x < 0)
            {
                gameObject.transform.GetChild(0).transform.localScale = new Vector2(-1, 1);
                helmetAnimator.transform.localScale = new Vector2(-1, 1);
                chestAnimator.transform.localScale = new Vector2(-1, 1);
                legsAnimator.transform.localScale = new Vector2(-1, 1);
                bootsAnimator.transform.localScale = new Vector2(-1, 1);
            }
            else if (_input.x > 0)
            {
                gameObject.transform.GetChild(0).transform.localScale = new Vector2(1, 1);
                helmetAnimator.transform.localScale = new Vector2(1, 1);
                chestAnimator.transform.localScale = new Vector2(1, 1);
                legsAnimator.transform.localScale = new Vector2(1, 1);
                bootsAnimator.transform.localScale = new Vector2(1, 1);
            }
            
            var state = _rb.velocity.magnitude < .25f  ? _idleHash : _runHash;
            _animator.CrossFade(state,0f);
            
            var armorState = _rb.velocity.magnitude < .25f  ? _helmetIdleHash : _helmetRunHash;
            helmetAnimator.CrossFade(armorState,0f);
            armorState = _rb.velocity.magnitude < .25f  ? _chestplateIdleHash : _chestplateRunHash;
            chestAnimator.CrossFade(armorState,0f);
            armorState = _rb.velocity.magnitude < .25f  ? _leggingsIdleHash : _leggingsRunHash;
            legsAnimator.CrossFade(armorState,0f);
            armorState = _rb.velocity.magnitude < .25f  ? _bootsIdleHash : _bootsRunHash;
            bootsAnimator.CrossFade(armorState,0f);
        }

        private void Movement()
        {
            if (_isRolling || _isFrozen)
                return;
            _rb.AddForce(_input.normalized * speed * Time.deltaTime);

            if(isCharging)
                _inputMovement = (Mathf.Clamp(_rb.velocity.magnitude, 0f, walkLimit) * _rb.velocity.normalized)/2;
            else 
                _inputMovement = Mathf.Clamp(_rb.velocity.magnitude, 0f, walkLimit) * _rb.velocity.normalized;
            _inputMovement -= _inputMovement * (25 * Time.deltaTime);
            _knockbackMovement -= _knockbackMovement * (10 * Time.deltaTime);
            
            _rb.velocity = _inputMovement + _knockbackMovement;
        }

        private void HandleStamina()
        {
            stamina = Mathf.Clamp(stamina += Time.deltaTime * 10f, 0f, staminaMax);
            _uiManager.UpdateStaminaBar(stamina / staminaMax);
        }

        private IEnumerator Roll()
        {
            if(_isFrozen || _openUI)
                yield break;
            _isRolling = true;
            canMove = false;
            _playerAttack.SetOpenOverlay(true);

            stamina -= _rollCost;
            _rb.velocity = (_input.normalized * rollSpeed);
            _input = Vector2.zero;
            
            _animator.CrossFade(_rollHash,0f);
            helmetAnimator.CrossFade(_helmetRollHash,0f);
            chestAnimator.CrossFade(_chestplateRollHash,0f);
            legsAnimator.CrossFade(_leggingsRollHash,0f);
            bootsAnimator.CrossFade(_bootsRollHash,0f);

            _playerHealth.SetInvincible(true);
            yield return new WaitForSeconds(.1f);
            _playerHealth.SetInvincible(false);
            yield return new WaitForSeconds(_rollTime - .1f);

            _playerAttack.SetOpenOverlay(false);
            _rb.velocity = Vector2.zero;
            _isRolling = false;
            canMove = true;
        }

        public void Push(Vector3 pos, float strength)
        {
            _rb.AddForce((transform.position - pos).normalized * strength, ForceMode2D.Impulse);
        }

        public void SetOpenUI(bool open)
        {
            _openUI = open;
            canMove = !open;
        }

        public void UpdateStats()
        {
            speed = defaultSpeed + GetComponent<Player>().GetBonusMoveSpeed();
            staminaMax = GetComponent<Player>().GetMaxMana();
            stamina = staminaMax;
        }

        public void UpdateArmorHashes()
        {
            if (_inventory == null)
                return;
            if (_inventory.GetEquippedHelmetScript() != null && _inventory.GetEquippedHelmetScript().GetAnimationHash() != "")
            {
                _helmetIdleHash = Animator.StringToHash(_inventory.GetEquippedHelmetScript().GetAnimationHash() + "player_idle");
                _helmetRunHash = Animator.StringToHash(_inventory.GetEquippedHelmetScript().GetAnimationHash() + "player_running");
                _helmetRollHash = Animator.StringToHash(_inventory.GetEquippedHelmetScript().GetAnimationHash() + "player_roll");
                helmetAnimator.CrossFade("player_idle",0f);
            }
            else
            {
                _helmetIdleHash = Animator.StringToHash("player_idle");
                _helmetRunHash = Animator.StringToHash("player_running");
                _helmetRollHash = Animator.StringToHash("player_roll");
            }
            
            if (_inventory.GetEquippedChestplateScript() != null && _inventory.GetEquippedChestplateScript().GetAnimationHash() != "")
            {
                _chestplateIdleHash = Animator.StringToHash(_inventory.GetEquippedChestplateScript().GetAnimationHash() + "player_idle");
                _chestplateRunHash = Animator.StringToHash(_inventory.GetEquippedChestplateScript().GetAnimationHash() + "player_running");
                _chestplateRollHash = Animator.StringToHash(_inventory.GetEquippedChestplateScript().GetAnimationHash() + "player_roll");
                chestAnimator.CrossFade("player_idle",0f);
            }
            else
            {
                _chestplateIdleHash = Animator.StringToHash("player_idle");
                _chestplateRunHash = Animator.StringToHash("player_running");
                _chestplateRollHash = Animator.StringToHash("player_roll");
            }

            if (_inventory.GetEquippedLeggingsScript() != null && _inventory.GetEquippedLeggingsScript().GetAnimationHash() != "")
            {
                _leggingsIdleHash = Animator.StringToHash(_inventory.GetEquippedLeggingsScript().GetAnimationHash() + "player_idle");
                _leggingsRunHash = Animator.StringToHash(_inventory.GetEquippedLeggingsScript().GetAnimationHash() + "player_running");
                _leggingsRollHash = Animator.StringToHash(_inventory.GetEquippedLeggingsScript().GetAnimationHash() + "player_roll");
                legsAnimator.CrossFade("player_idle",0f);
            }
            else
            {
                _leggingsIdleHash = Animator.StringToHash("player_idle");
                _leggingsRunHash = Animator.StringToHash("player_running");
                _leggingsRollHash = Animator.StringToHash("player_roll");
            }
            
            if (_inventory.GetEquippedBootsScript() != null && _inventory.GetEquippedBootsScript().GetAnimationHash() != "")
            {
                _bootsIdleHash = Animator.StringToHash(_inventory.GetEquippedBootsScript().GetAnimationHash() + "player_idle");
                _bootsRunHash = Animator.StringToHash(_inventory.GetEquippedBootsScript().GetAnimationHash() + "player_running");
                _bootsRollHash = Animator.StringToHash(_inventory.GetEquippedBootsScript().GetAnimationHash() + "player_roll");
                bootsAnimator.CrossFade("player_idle",0f);
            }
            else
            {
                _bootsIdleHash = Animator.StringToHash("player_idle");
                _bootsRunHash = Animator.StringToHash("player_running");
                _bootsRollHash = Animator.StringToHash("player_roll");
            }
            
            _animator.CrossFade("player_roll",0f);
        }

        public void FreezeFor(float getDuration)
        {
            StartCoroutine(Freeze(getDuration));
        }
        
        private IEnumerator Freeze(float getDuration)
        {
            _isFrozen = false;
            yield return new WaitForSeconds(getDuration);
            _isFrozen = true;
        }

        public void UnFreeze()
        {
            _isFrozen = false;
        }
    }
}