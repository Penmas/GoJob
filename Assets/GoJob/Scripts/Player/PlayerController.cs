using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
	[Header("�ɷ�ġ")]
	[SerializeField] private float maxSpeed;                                // �÷��̾� �ְ� �ӵ�
	[SerializeField] private float acceleration;							// ����
	[SerializeField] private float decelerationLevel;                       // ����
	[SerializeField] private float turnSpeed;
	[SerializeField] private float aliveTime;

	// ������Ʈ
	private Rigidbody _rigidbody;


	// ��ġ
	private float currentAngle;
	private float currentIdleTime;

	private Vector3 playerForward;
	private float angle;


	public Vector3 PlayerForward
	{
		set => playerForward = value;
		get => playerForward;
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();


		currentAngle = transform.rotation.y;

	}


	private void Update()
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


		// �ð�
		currentIdleTime += Time.deltaTime;
		
		
		//ȸ��
		if (Ypos != 0)
		{
			float time = Time.deltaTime;

			currentAngle += Ypos * turnSpeed * time;
			angle = transform.eulerAngles.y + (Ypos * turnSpeed * time);
			if (currentAngle < -180)
			{
				currentAngle += 360;
			}
			else if (currentAngle > 180)
			{
				currentAngle -= 360;
			}


			transform.eulerAngles = new Vector3(_rigidbody.rotation.eulerAngles.x,
									angle,
									_rigidbody.rotation.eulerAngles.z);
			//Debug.Log(currentAngle);
		}
		else
		{

		}



		if (xPos != 0)
		{
			playerForward = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward;

			_rigidbody.velocity += playerForward * xPos * Time.deltaTime * acceleration;


			AccelerationLimit();



		}

	}

	// �÷��̾� �ӷ� ����
	private void AccelerationLimit()
	{


		if (_rigidbody.velocity.x > maxSpeed)
		{
			_rigidbody.velocity = new Vector3(maxSpeed, _rigidbody.velocity.y, _rigidbody.velocity.z);

			return;
		}

		if (_rigidbody.velocity.x < (maxSpeed * -1))
		{
			_rigidbody.velocity = new Vector3((maxSpeed * -1), _rigidbody.velocity.y, _rigidbody.velocity.z);
			return;

		}

		if (_rigidbody.velocity.z > maxSpeed)
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, maxSpeed);
			return;

		}

		if (_rigidbody.velocity.z < (maxSpeed * -1))
		{
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, (maxSpeed * -1));
			return;
		}

		//Debug.Log(_rigidbody.velocity);
		
	}

	// �÷��̾� ����
	private void PlayerSpeedDeceleration()
	{
		Vector3 currentVelocity = _rigidbody.velocity;
		currentVelocity -= currentVelocity * decelerationLevel * Time.deltaTime;
		_rigidbody.velocity = currentVelocity;
	}




	// �÷��̾� �ð� ����
	private void playerDeathTimeCheck()
	{

	}
}
