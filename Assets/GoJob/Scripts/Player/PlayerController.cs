using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
	[Header("능력치")]
	[SerializeField] private float maxSpeed;                                // 플레이어 최고 속도
	[SerializeField] private float acceleration;							// 가속
	[SerializeField] private float decelerationLevel;                       // 감속
	[SerializeField] private float turnSpeed;


	// 컴포넌트
	private Rigidbody _rigidbody;


	// 수치
	private float currentSpeed;                     // 현재 플레이어의 속도
	private float deceleration;                     // 플레이어 감속
	private float currentAngle;


	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();


		currentAngle = transform.rotation.y;

	}


	private void FixedUpdate()
	{
		// 플레이어 움직임
		PlayerMove();

		// 플레이어 자동 감속
		PlayerSpeedDeceleration();



	}




	// 플레이어 움직이기
	private void PlayerMove()
	{
		// 앞뒤 이동
		float xPos = Input.GetAxis("Vertical");
		float Ypos = Input.GetAxis("Horizontal");




		//회전
		if (Ypos != 0)
		{
			currentAngle = _rigidbody.rotation.eulerAngles.y + (Ypos * Time.deltaTime * turnSpeed);
			if (currentAngle < 0)
			{
				currentAngle += 360;
			}
			else if (currentAngle > 360)
			{
				currentAngle -= 360;
			}
			transform.eulerAngles = new Vector3(_rigidbody.rotation.eulerAngles.x,
									currentAngle,
									_rigidbody.rotation.eulerAngles.y);
		}

		if (xPos != 0)
		{
			Debug.Log(xPos);
			Vector3 playerForward = Quaternion.Euler(0f, currentAngle, 0) * Vector3.forward;


			if(!AccelerationLimit())
			{
				_rigidbody.velocity += playerForward * xPos * Time.deltaTime * acceleration;
			}
			

		}

	}

	// 플레이어 속력 제한
	private bool AccelerationLimit()
	{

		bool isMaxSpeed = false;

		if (_rigidbody.velocity.x > maxSpeed)
		{
			_rigidbody.velocity = new Vector3(maxSpeed, _rigidbody.velocity.y, _rigidbody.velocity.z);

			isMaxSpeed = true;
		}

		if (_rigidbody.velocity.x < (maxSpeed * -1))
		{
			_rigidbody.velocity = new Vector3((maxSpeed * -1), _rigidbody.velocity.y, _rigidbody.velocity.z);
			isMaxSpeed = true;

		}

		if (_rigidbody.velocity.z > maxSpeed)
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, maxSpeed);
			isMaxSpeed = true;

		}

		if (_rigidbody.velocity.z < (maxSpeed * -1))
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, (maxSpeed * -1));
			isMaxSpeed = true;
		}

		//Debug.Log(_rigidbody.velocity);
		return isMaxSpeed;
	}


	private void PlayerSpeedDeceleration()
	{
		Vector3 currentVelocity = _rigidbody.velocity;
		currentVelocity -= currentVelocity * decelerationLevel * Time.deltaTime;
		_rigidbody.velocity = currentVelocity;
	}

}
