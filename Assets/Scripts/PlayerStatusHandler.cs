using RootMotion.FinalIK;
using UnityEngine;


namespace Game2D
{
    public sealed class PlayerStatusHandler : MonoBehaviour, IDieble
    {

        [SerializeField] private SoundPlayer _soundPlayer;
        [SerializeField] private ParticleSystem _deathEffect;
        [SerializeField] private float _health;


        private EventHandler _eventHandler;

        public float Health
        {
            get => _health;
            set
            {
                _eventHandler.TriggerEvent(EventType.DMG_RECEIVE, "" + (_health - value));
                _health = value; 
            }
        }

        private void Start()
        {
            _eventHandler = GameObject.FindGameObjectWithTag("GUI").GetComponent<EventHandler>();

        }

        private void Update()
        {       
        }

        public void Die()
        {
            print("U DEAD!!!");
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            _deathEffect.gameObject.SetActive(true);
            _deathEffect.Play();
            _deathEffect.loop = false;
            _soundPlayer.PlaySound(SoundEffectType.PlayerDieing, true);
        }
    }

}