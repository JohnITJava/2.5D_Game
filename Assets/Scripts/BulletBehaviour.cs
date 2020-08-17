using UnityEngine;


namespace Game2D
{
    public class BulletBehaviour : MonoBehaviour
    {

        [SerializeField] private float _energyBulletDamage;

        private SoundPlayer _soundPlayer;

        private GameObject _parentBox;
        private Transform _mapObject;


        private void Start()
        {
            _soundPlayer = GameObject.FindGameObjectWithTag("SoundPlayer").GetComponent<SoundPlayer>();
            _parentBox = transform.parent.parent.gameObject;
            _mapObject = GameObject.FindGameObjectWithTag("Map").gameObject.transform;          
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_parentBox.CompareTag("AmmoBox"))
            {
                Destroy(gameObject, 10.0f);

                if (collision.gameObject.CompareTag("Turret"))
                {
                    var gun = gameObject.GetComponentInParent<EnergyRevolver>();
                    if (gun != null)
                    {
                        gun.Damage(collision.gameObject.GetComponentInChildren<IDieble>());
                    }                                      
                }
                else
                {
                    _soundPlayer.PlaySound(SoundEffectType.BulletMilk, true);
                }


                gameObject.transform.SetParent(_mapObject);
            }
        }

        public float BulletDamage
        {
            get => _energyBulletDamage;
        }
    }
}