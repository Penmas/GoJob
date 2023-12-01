using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("�ɷ�ġ")]
	[SerializeField] private float maxSpeed;                                // �÷��̾� �ְ� �ӵ�
	[SerializeField] private float acceleration;							// ����
	[SerializeField] private float decelerationLevel;                       // ����



	// ������Ʈ
	private Rigidbody _rigidbody;


	// ��ġ
	private float currentSpeed;                     // ���� �÷��̾��� �ӵ�
	private float deceleration;                     // �÷��̾� ����



	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}


	private void Update()
	{

		PlayerMove();

		PlayerSpeedDeceleration();
	}



	// �÷��̾� �����̱�
	private void PlayerMove()
	{
		float xPos = Input.GetAxisRaw("Horizontal");
		float zPos = Input.GetAxisRaw("Vertical");

		Vector3 moveForce = new Vector3(xPos * 2, 0, zPos) * Time.deltaTime * acceleration * 20f;
		_rigidbody.AddForce(moveForce, ForceMode.Force);                                    // ���Ը� �����Ͽ� �������� ���� ��
		AccelerationLimit();
	}

	// �÷��̾� �ӷ� ����
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
