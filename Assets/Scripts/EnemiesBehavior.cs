using JetBrains.Annotations;
using UnityEngine;

public class EnemiesBehavior : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private LayerMask _playerMask;
    private float _attackRange = 2f;

    [Header("Audio")]
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] AudioClip maceSwingSFX;

    [Header("Scripts")]
    [SerializeField] private PlayerBehavior _player;

    #region Class
    private Rigidbody2D _enemyRb;
    private Animator _enemyAnimator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _groundCollider;
    #endregion

    #region Struct
    private RaycastHit2D _hit;
    private Vector3 _initialPosition;
    private Vector3 _playerInitialPosition;

    private bool _isAlive = true;
    private bool _isDying = false;
    [HideInInspector] public bool isAttacking = false;
    #endregion

    private void Awake()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _groundCollider = GetComponent<BoxCollider2D>();
        _playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        if (_isAlive)
        {
            OnAlive();
            Move();
            AttackPlayer();
        }
    }

    private void Move()
    {
        _enemyRb.velocity = new Vector2(_moveSpeed, 0f);
        _enemyAnimator.SetBool("isMove", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _moveSpeed = -_moveSpeed;
        FlipEnemy();
    }

    private void FlipEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_enemyRb.velocity.x)), 1f);
    }

    private void OnAlive()
    {
        _enemyAnimator.SetBool("isAlive", true);
    }

    public void TakeDamage()
    {
        if (!_isDying && _isAlive)
        {
            float delayEnemyDeath = 0.25f;

            _isDying = true;
            _isAlive = false;
            _enemyAnimator.SetBool("isAlive", _isAlive);
            _enemyAnimator.SetTrigger("Hit");
            _enemyRb.velocity = Vector2.zero;
            _enemyRb.gravityScale = 0f;
            _enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;

            Invoke(nameof(PlayDeathAnimation), delayEnemyDeath);
        }
    }

    private void PlayDeathAnimation()
    {
        _enemyAnimator.SetTrigger("Die");
        AudioManager.Instance.sfxAudioSource.PlayOneShot(enemyDeathSFX);

        _enemyRb.velocity = Vector2.zero;
        _bodyCollider.enabled = false;
        _groundCollider.enabled = false;

        Invoke(nameof(OnDeathAnimationComplete), 1.5f);
    }

    public void OnDeathAnimationComplete()
    {
        _enemyRb.bodyType = RigidbodyType2D.Kinematic;
        _isDying = false;
    }

    public void AttackPlayer()
    {
        if (!_isAlive) return;

        Vector2 lookDirection = (_moveSpeed > 0) ? transform.right : -transform.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDirection, _attackRange, _playerMask);

        if (hit.collider != null && _playerMask == (_playerMask | (1 << hit.collider.gameObject.layer)))
        {
            float distanceToPlayer = hit.distance;

            Debug.DrawLine(transform.position, _playerInitialPosition, Color.red, _attackRange);

            if (distanceToPlayer < _attackRange && _player._isAlive && !isAttacking)
            {
                _playerInitialPosition = _player.transform.position;
                isAttacking = true;
                _enemyAnimator.SetTrigger("Attack");
                Invoke("CheckPlayerDistance", 0.95f);
            }
        }
    }

    private void CheckPlayerDistance()
    {
        if (!_isAlive) return;

        float distanceToPlayer = Vector3.Distance(_playerInitialPosition, _player.transform.position);

        Debug.DrawLine(transform.position, _playerInitialPosition, Color.green, _attackRange);

        if (distanceToPlayer < _attackRange && _player._isAlive && !_isAlive)
        {
            _player.TakeDamage();
            AudioManager.Instance.gameAudioSource.PlayOneShot(maceSwingSFX);
        }

        isAttacking = false;
    }
}