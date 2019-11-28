using System.Collections;
using System.Collections.Generic;
using TBD.Camera;
using UnityEngine;

namespace TBD.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] int hackableLayerId = 11;          // TODO: need to make the value constant
        // [SerializeField] Weapon weapon;
        [SerializeField] AnimatorOverrideController animationOverride;

        [Header("Obsolete")]
        [SerializeField] float currentHealthPoints = 100f;
        [SerializeField] float maxHealthPoints = 100f;

        private CameraRaycaster raycaster = null;

        // TODO:  because we are importing unity class package for health, this property
        // has to start lower case, against standard C# conventions
        public float healthAsPercentage
        {
            get
            {
                return PlayerState.Instance.CurrentHealth / PlayerState.Instance.MaxHealth;
            }
        }

        private void Awake()
        {
            print("Player State staleness is " + PlayerState.Instance.When);
        }

        // Use this for initialization
        void Start()
        {
            raycaster = FindObjectOfType<CameraRaycaster>();
            raycaster.NotifyMouseClickObservers += Raycaster_NotifyMouseClickObservers;

            OverrideAnimationController();
        }

        // Update is called once per frame
        void Update() { }

        private void OverrideAnimationController()
        {
            /*
            if (null == animationOverride)
                return;

            Animator animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animationOverride;

            // https://www.explosive.ws/products/rpg-character-mecanim-animation-pack
            // TODO:  override animations:  lecture 131, @11:39
            // TODO:  consider overriding animation events:  lecture 132: 7:50
            */
        }

        private void Raycaster_NotifyMouseClickObservers(RaycastHit raycastHit, int layerHit)
        {
            if (hackableLayerId == layerHit)
            {
                var hackable = raycastHit.collider.gameObject;
                print("clicked on a hackable item " + hackable);
            }
        }
    }
}