using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("능력치")]
	[SerializeField] private float maxSpeed;                                // 플레이어 최고 속도
	[SerializeField] private float acceleration;							// 가속
	[SerializeField] private float decelerationLevel;                       // 감속



	// 컴포넌트
	private Rigidbody _rigidbody;


	// 수치
	private float currentSpeed;                     // 현재 플레이어의 속도
	private float deceleration;                     // 플레이어 감속



	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}


	private void Update()
	{

		PlayerMove();

		PlayerSpeedDeceleration();
	}



	// 플레이어 움직이기
	private void PlayerMove()
	{
		float xPos = Input.GetAxisRaw("Horizontal");
		float zPos = Input.GetAxisRaw("Vertical");

		Vector3 moveForce = new Vector3(xPos * 2, 0, zPos) * Time.deltaTime * acceleration * 20f;
		_rigidbody.AddForce(moveForce, ForceMode.Force);                                    // 무게를 적용하여 순간적인 힘을 줌
		AccelerationLimit();
	}

	// 플레이어 속력 제한
	private void AccelerationLimit()
	{

	}


	private void PlayerSpeedDeceleration()
	{
		Vector3 currentVelocity = _rigidbody.velocity;
		currentVelocity -= currentVelocity * decelerationLevel * Time.deltaTime;
		_rigidbody.velocity = currentVelocity;
	}

}
