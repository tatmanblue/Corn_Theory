using UnityEngine;

namespace CornTheory.Player
{
	/// <summary>
	/// this class replaces  UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter
	/// </summary>
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class CornTheoryThirdPersonCharactor : MonoBehaviour
	{
		[SerializeField] float _movingTurnSpeed = 360;
		[SerializeField] float _stationaryTurnSpeed = 180;
		[SerializeField] float _jumpPower = 6f;
		[Range(1f, 4f)] [SerializeField] float _gravityMultiplier = 2f;
		[SerializeField] float _runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float _moveSpeedMultiplier = 2.0f;
		[SerializeField] float _animSpeedMultiplier = 1f;
		[SerializeField] float _groundCheckDistance = 0.3f;

		const float HALF = 0.5f;

		Rigidbody _rigidbody;
		Animator _animator;
		bool isGrounded;
		float _origGroundCheckDistance;
		float _turnAmount;
		float _forwardAmount;
		Vector3 _groundNormal;
		float _capsuleHeight;
		Vector3 _capsuleCenter;
		CapsuleCollider _capsule;
		bool _isCrouching;


		void Start()
		{
			_animator = GetComponent<Animator>();
			_rigidbody = GetComponent<Rigidbody>();
			_capsule = GetComponent<CapsuleCollider>();
			_capsuleHeight = _capsule.height;
			_capsuleCenter = _capsule.center;

			_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			_origGroundCheckDistance = _groundCheckDistance;
		}


		public void Move(Vector3 move, bool crouch, bool jump)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, _groundNormal);
			_turnAmount = Mathf.Atan2(move.x, move.z);
			_forwardAmount = move.z;

			ApplyExtraTurnRotation();

			// control and velocity handling is different when grounded and airborne:
			if (isGrounded)
			{
				HandleGroundedMovement(crouch, jump);
			}
			else
			{
				HandleAirborneMovement();
			}

			ScaleCapsuleForCrouching(crouch);
			PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}


		void ScaleCapsuleForCrouching(bool crouch)
		{
			if (isGrounded && crouch)
			{
				if (_isCrouching) return;
				_capsule.height = _capsule.height / 2f;
				_capsule.center = _capsule.center / 2f;
				_isCrouching = true;
			}
			else
			{
				Ray crouchRay = new Ray(_rigidbody.position + Vector3.up * _capsule.radius * HALF, Vector3.up);
				float crouchRayLength = _capsuleHeight - _capsule.radius * HALF;
				if (Physics.SphereCast(crouchRay, _capsule.radius * HALF, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					_isCrouching = true;
					return;
				}
				_capsule.height = _capsuleHeight;
				_capsule.center = _capsuleCenter;
				_isCrouching = false;
			}
		}

		void PreventStandingInLowHeadroom()
		{
			// prevent standing up in crouch-only zones
			if (!_isCrouching)
			{
				Ray crouchRay = new Ray(_rigidbody.position + Vector3.up * _capsule.radius * HALF, Vector3.up);
				float crouchRayLength = _capsuleHeight - _capsule.radius * HALF;
				if (Physics.SphereCast(crouchRay, _capsule.radius * HALF, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
				{
					_isCrouching = true;
				}
			}
		}


		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			_animator.SetFloat("Forward", _forwardAmount, 0.1f, Time.deltaTime);
			_animator.SetFloat("Turn", _turnAmount, 0.1f, Time.deltaTime);
			_animator.SetBool("Crouch", _isCrouching);
			_animator.SetBool("OnGround", isGrounded);
			if (!isGrounded)
			{
				_animator.SetFloat("Jump", _rigidbody.velocity.y);
			}

			// calculate which leg is behind, so as to leave that leg trailing in the jump animation
			// (This code is reliant on the specific run cycle offset in our animations,
			// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
			float runCycle =
				Mathf.Repeat(
					_animator.GetCurrentAnimatorStateInfo(0).normalizedTime + _runCycleLegOffset, 1);
			float jumpLeg = (runCycle < HALF ? 1 : -1) * _forwardAmount;
			if (isGrounded)
			{
				_animator.SetFloat("JumpLeg", jumpLeg);
			}

			// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
			// which affects the movement speed because of the root motion.
			if (isGrounded && move.magnitude > 0)
			{
				_animator.speed = _animSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				_animator.speed = 1;
			}
		}


		void HandleAirborneMovement()
		{
			// apply extra gravity from multiplier:
			Vector3 extraGravityForce = (Physics.gravity * _gravityMultiplier) - Physics.gravity;
			_rigidbody.AddForce(extraGravityForce);

			_groundCheckDistance = _rigidbody.velocity.y < 0 ? _origGroundCheckDistance : 0.01f;
		}


		void HandleGroundedMovement(bool crouch, bool jump)
		{
			// check whether conditions are right to allow a jump:
			if (jump && !crouch && _animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
			{
				// jump!
				_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpPower, _rigidbody.velocity.z);
				isGrounded = false;
				_animator.applyRootMotion = false;
				_groundCheckDistance = 0.1f;
			}
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(_stationaryTurnSpeed, _movingTurnSpeed, _forwardAmount);
			transform.Rotate(0, _turnAmount * turnSpeed * Time.deltaTime, 0);
		}


		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (isGrounded && Time.deltaTime > 0)
			{
				Vector3 v = (_animator.deltaPosition * _moveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				v.y = _rigidbody.velocity.y;
				_rigidbody.velocity = v;
			}
		}


		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * _groundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, _groundCheckDistance))
			{
				_groundNormal = hitInfo.normal;
				isGrounded = true;
				_animator.applyRootMotion = true;
			}
			else
			{
				isGrounded = false;
				_groundNormal = Vector3.up;
				_animator.applyRootMotion = false;
			}
		}
	}
}
