using UnityEngine;


namespace Game2D
{
    public sealed class GatlingGun : MonoBehaviour, IDieble, IDamagable
    {
        #region Fields

        [SerializeField] private SoundPlayer _soundPlayer;

        [SerializeField] private GameObject _bulletSpawn;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private Transform go_baseRotation;
        [SerializeField] private Transform go_GunBody;
        [SerializeField] private Transform go_barrel;

        [SerializeField] private float _barrelRotationSpeed;
        [SerializeField] private float _firingRange;
        [SerializeField] private float _health;
        [SerializeField] private float _damage;
        [SerializeField] private float _reloadTimeout;
        [SerializeField] private float _startShootSpeed;

        private EventHandler _eventHandler;

        private Transform _go_target;

        private Quaternion _baseRotation = new Quaternion(0f, -1f, 0f, 1);
        private Quaternion _targetRotation;
        private Vector3 _relativeRotate;
        private Vector3 _targetVector;

        private bool _canFire = false;
        private bool _isAlive;
        private float _currentRotationSpeed;
        private float _deltaAngle;
        private float _timer;

        #endregion


        #region Properties

        public GameObject BulletSpawn
        {
            get => _bulletSpawn;
        }

        public Transform BaseRotator
        {
            get => go_baseRotation;
        }

        public float Health
        {
            get => _health;
            set => _health = value;
        }

        public float BulletStartSpeed
        {
            get => _startShootSpeed;
        }

        public int Magazine { get => 0; }
        public bool IsItemInHand { set => throw new System.NotImplementedException(); }

        #endregion


        #region UnityMethods

        private void Start()
        {
            _currentRotationSpeed = _barrelRotationSpeed;
            this.GetComponent<SphereCollider>().radius = _firingRange / gameObject.transform.localScale.x;
            _eventHandler = GameObject.FindGameObjectWithTag("GUI").GetComponent<EventHandler>();
            _isAlive = true;
        }

        private void Update()
        {
            if (_isAlive)
            {
                _timer += Time.deltaTime;
                AimAndFire();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _firingRange);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _go_target = other.transform;
                _canFire = true;
                _timer = 0.0f;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _canFire = false;
            }
        }

        #endregion


        #region Methods

        private void AimAndFire()
        {
            go_barrel.Rotate(0, 0, _currentRotationSpeed * 500 * Time.deltaTime);

            if (_canFire)
            {
                _currentRotationSpeed = _barrelRotationSpeed;

                _targetVector = _go_target.position - go_baseRotation.position;
                _deltaAngle = Mathf.Atan2(_targetVector.x, _targetVector.z) * Mathf.Rad2Deg;
                _targetRotation = Quaternion.AngleAxis(_deltaAngle, Vector3.up);

                go_baseRotation.rotation = Quaternion.Slerp(go_baseRotation.rotation, _targetRotation, _barrelRotationSpeed * Time.deltaTime);

                ShootProcess();
            }
            else
            {
                _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, 0, 10 * Time.deltaTime);
                go_baseRotation.rotation = Quaternion.Slerp(go_baseRotation.rotation, _baseRotation, _barrelRotationSpeed * Time.deltaTime);

            }
        }

        private void ShootProcess()
        {
            if (_timer >= _reloadTimeout)
            {
                var bullet = Instantiate(_bullet, _bulletSpawn.transform.position, _bulletSpawn.transform.rotation);
                _timer = 0.0f;

                _soundPlayer.PlaySound(SoundEffectType.TurretShooting, true);
            }
        }

        public void Die()
        {
            _isAlive = false;
            var rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(transform.up * 0.1f, ForceMode.Impulse);

            transform.rotation = Quaternion.AngleAxis(-35.0f, Vector3.right);
            transform.rotation = Quaternion.AngleAxis(35.0f, Vector3.forward);

            _soundPlayer.PlaySound(SoundEffectType.TurretDieing, true);

            Destroy(gameObject, 5.0f);
        }

        public void Damage(IDieble enemy)
        {
            enemy.Health -= _damage;

            var enemyObject = ((Component)enemy).transform.gameObject;
            if (enemyObject.CompareTag("Player"))
            {
                _eventHandler.TriggerEvent(EventType.DMG_RECEIVE, "" + _damage);
            }
            else
            {
                _eventHandler.TriggerEvent(EventType.INFO, $"{this.name} ATTACKS {enemyObject.name}");
            }

            if (enemy.Health <= 0)
            {
                enemy.Die();
                _eventHandler.TriggerEvent(EventType.SMB_KILLED, $"{enemyObject.tag} by Turret");
                _isAlive = false;
            }
        }

        #endregion
    }
}