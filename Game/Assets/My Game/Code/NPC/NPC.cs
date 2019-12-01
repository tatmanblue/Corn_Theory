using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace CornTheory.NonPlayer
{
	public class NPC : MonoBehaviour
	{
		[SerializeField] bool drawGizmos = false;
		[SerializeField] float chaseRadius = 0f;                    // how close the player must get before NPC chases player
		[SerializeField] float stoppingRadius = 1f;                 // distance NPC stands next to player
        [SerializeField] float breakRadius = 1f;                    // distance to break off chasing player and return "home"
        [SerializeField] bool chasePlayer = true;

		[Header("Weapon")]
		// [SerializeField] Weapon weapon;
		[SerializeField] GameObject weaponAttachPoint;

		[Header("Missions")]
		[SerializeField] int missionId = 0;                         // converstation or other interaction satifies mission

		AICharacterControl characterControl = null;
		Vector3 homePosition;
		GameObject player = null;

		private void OnDrawGizmos()
		{
			if (false == drawGizmos)
				return;

			if (true == chasePlayer && 0 < chaseRadius)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(transform.position, chaseRadius);
			}

            if (true == chasePlayer && 0 < breakRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, breakRadius);
            }
		}

		void Start ()
		{
			player = GameObject.FindGameObjectWithTag("Player");
			characterControl = GetComponent<AICharacterControl>();
			homePosition = transform.position;

			PutWeaponInHand();
		}
		
		void Update ()
		{
			MoveToPlayer();
		}

		GameObject RequestWeaponAttachPoint()
		{
            // DominateHand[] possibleAttachPoints = GetComponentsInChildren<DominateHand>();
            //if (0 == possibleAttachPoints.Length)
            //	return null;

            //return possibleAttachPoints[0].gameObject;
            return null;
		}

		void PutWeaponInHand()
		{
			//if (null == weaponAttachPoint)
			//{
			//	weaponAttachPoint = RequestWeaponAttachPoint();
			//}

			//if (null == weapon || null == weaponAttachPoint)
			//	return;

			//GameObject weaponPrefab = weapon.GetWeaponPrefab();
			//if (null == weaponPrefab)
			//	return;

			//GameObject weaponInstance = Instantiate(weaponPrefab, weaponAttachPoint.transform);

			//weaponInstance.transform.localPosition = weapon.GripTransform.localPosition;
			//weaponInstance.transform.localRotation = weapon.GripTransform.localRotation;
		}

		float GetDistanceToPlayer()
		{
			return Vector3.Distance(player.transform.position, transform.position);
		}

		void MoveToPlayer()
		{
			if (false == chasePlayer || 0 == chaseRadius)
				return;

			float distanceToPlayer = GetDistanceToPlayer();

			if (distanceToPlayer < chaseRadius && stoppingRadius < distanceToPlayer)
				characterControl.target = player.transform;
			else
				characterControl.target = null;
		}

	}
}