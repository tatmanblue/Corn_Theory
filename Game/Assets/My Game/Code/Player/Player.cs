using System.Collections;
using System.Collections.Generic;
using CornTheory.Camera;
using CornTheory.Scriptables;
using UnityEngine;

namespace CornTheory.Player
{
    /// <summary>
    /// Represents Cam, the player object
    /// TODO: Need to think through some things:  Player object should be about the state of the GameObject
    /// and PlayerState should be about attributes that affect the Player game object
    /// TODO: any public methods should be part of an interface so that consumers reference by interface
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] InventoryDatabaseObject inventoryDB;
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

        // -----------------------------------------------------------
        // public methods
        // -----------------------------------------------------------
        public void AddInventoryItem(InventoryDescriptionObject item, int qty)
        {
            if (null == inventoryDB)
                return;

            inventoryDB.AddInventoryItem(item, qty);
        }

        // -----------------------------------------------------------
        // Unity MonoBehavior Overrides
        // -----------------------------------------------------------
        private void Awake()
        {
            print("Player State staleness is " + PlayerState.Instance.When);
        }

        // Use this for initialization
        private void Start()
        {
            // raycaster = FindObjectOfType<CameraRaycaster>();
            // raycaster.NotifyMouseClickObservers += Raycaster_NotifyMouseClickObservers;

            OverrideAnimationController();
        }

        // Update is called once per frame
        private void Update() { }

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