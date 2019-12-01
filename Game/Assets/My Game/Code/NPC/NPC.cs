using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace TBD.NonPlayer
{
	public class NPC : MonoBehaviour
	{
		[SerializeField] bool drawGizmos = false;
		[SerializeField] float chaseRadius = 0f;
		[SerializeField] float stoppingRadius = 1f;
		[SerializeField] bool chasePlayer = true;

		// TODO:  this should be removed now that we have a different system
		[Header("Speech Enginer - Obsolete")]
		[SerializeField] float speechRadius = 0f;
		[SerializeField] int speechTextId = 0;

		[Header("Weapon")]
		// [SerializeField] Weapon weapon;
		[SerializeField] GameObject weaponAttachPoint;

		[Header("Missions")]
		[SerializeField] int missionId = 0;

		// INPCSpeechEngine speechEngine = null;
		ThirdPersonCharacter character = null;
		AICharacterControl characterControl = null;
		Vector3 homePosition;
		GameObject player = null;
		bool isTalking = false;

		private void OnDrawGizmos()
		{
			if (false == drawGizmos)
				return;

			if (true == chasePlayer && 0 < chaseRadius)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(transform.position, chaseRadius);
			}

			if (0 < speechRadius && 0 < speechTextId)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.position, speechRadius);
			}

		}

		void Start ()
		{
			player = GameObject.FindGameObjectWithTag("Player");
			character = GetComponent<ThirdPersonCharacter>();
			characterControl = GetComponent<AICharacterControl>();
			homePosition = transform.position;

			PutWeaponInHand();
		}
		
		void Update ()
		{
			//if (null == speechEngine)
			//{
			//	speechEngine = GetComponent<INPCSpeechEngine>() as INPCSpeechEngine;
			//}

			MoveToPlayer();
			TalkToPlayer();
		}

		GameObject RequestWeaponAttachPoint()
		{
			// DominateHand[] possibleAttachPoints = GetComponentsInChildren<DominateHand>();
			//if (0 == possibleAttachPoints.Length)
			//	return null;

			//return possibleAttachPoints[0].gameObject;
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

		void TalkToPlayer()
		{
			if (0 == speechTextId) return;

			float distanceToPlayer = GetDistanceToPlayer();

			if (false == isTalking && distanceToPlayer < speechRadius)
			{
				isTalking = true;
				InvokeRepeating("SaySomethingToPlayer", 0.1f, 2.0F);
				return;
			}

			if (true == isTalking && distanceToPlayer >= speechRadius)
				isTalking = false;
		}

		private void CancelSaySomethingToPlayer()
		{
			CancelInvoke("SaySomethingToPlayer");
		}

		void SaySomethingToPlayer()
		{
			//if (null == speechEngine)
			//{
			//	CancelSaySomethingToPlayer();
			//	isTalking = false;
			//	return;
			//}

			//isTalking = true;
			//speechEngine.PrintTextOnScreen("say something");
		}


	}
}