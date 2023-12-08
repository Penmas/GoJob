using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
	[Header("�ɷ�ġ")]
	[SerializeField] private float maxSpeed;                                // �÷��̾� �ְ� �ӵ�
	[SerializeField] private float acceleration;							// ����
	[SerializeField] private float decelerationLevel;                       // ����
	[SerializeField] private float turnSpeed;


	// ������Ʈ
	private Rigidbody _rigidbody;


	// ��ġ
	private float currentSpeed;                     // ���� �÷��̾��� �ӵ�
	private float deceleration;                     // �÷��̾� ����
	private float currentAngle;


	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();


		currentAngle = transform.rotation.y;

	}


	private void FixedUpdate()
	{
		// �÷��̾� ������
		PlayerMove();

		// �÷��̾� �ڵ� ����
		PlayerSpeedDeceleration();



	}




	// �÷��̾� �����̱�
	private void PlayerMove()
	{
		// �յ� �̵�
		float xPos = Input.GetAxis("Vertical");
		float Ypos = Input.GetAxis("Horizontal");




		//ȸ��
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

	// �÷��̾� �ӷ� ����
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
