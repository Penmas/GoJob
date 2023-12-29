using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[Header("�ɷ�ġ")]
	[SerializeField] private Vector3 respawnPosition;
	[SerializeField] private float maxSpeed;                                // �÷��̾� �ְ� �ӵ�
	[SerializeField] private float acceleration;							// ����
	[SerializeField] private float decelerationLevel;                       // ����
	[SerializeField] private float turnSpeed;
	[SerializeField] private float aliveTime;

	[Space(10)]
	[Header("�𵨸�")]
	[SerializeField] private GameObject model;								// �𵨸�

	[Space(10)]
	[Header("���� ��ġ")]
	[SerializeField] private float currentSpeed;							// ���� �ӵ�
	[SerializeField] private float currentAngle;							// ���� ����
	[SerializeField] private float currentIdleTime;                         // �������� ���� �ð�

	[Space(10)]
	[Header("�̺�Ʈ")]
	[SerializeField] private UnityEvent gameclear;
	[SerializeField] private UnityEvent gameover;

	// ������Ʈ
	private Rigidbody _rigidbody;


	// ��ġ
	private float defaultAngle;
	private float angle;					// �𵨸��� ������ ����
	private float accelBlock;				// ���� ��� �ӵ�


	private bool isForwardMoved;			//������ �����̴��� �Ǵ�


	// ������Ƽ ����
	public float AccelBlock
	{
		set => accelBlock = value;
		get => accelBlock;
	}
	public float AliveTime
	{
		get => aliveTime;
	}
	public float CurrentIdleTime
	{
		get => currentIdleTime;
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();

		defaultAngle = transform.eulerAngles.y;
		currentAngle = defaultAngle;
		accelBlock = 1;
	}


	private void Update()
	{
		// �ð�
		currentIdleTime += Time.deltaTime;

		playerDeathTimeCheck();
		
		//���� �� üũ
		BlockCheck();

		// ���� ��ġ üũ
		OutCheck();
	}

	private void FixedUpdate()
	{
		// �÷��̾� ������
		PlayerMove();

		// �÷��̾� �𵨸� ȸ��
		ModelRotation();

		// �÷��̾� �ڵ� ����
		PlayerSpeedDeceleration();

	}




	// �÷��̾� �����̱�
	private void PlayerMove()
	{
		// �յ� �̵�
		float xPos = Input.GetAxis("Vertical");
		float yPos = Input.GetAxis("Horizontal");



		float time = Time.fixedDeltaTime;
		//float time = Time.deltaTime;

        // �̵�
		if(xPos != 0)
		{
			if (Mathf.Abs(currentSpeed) < maxSpeed)
			{
				currentSpeed += xPos * time * acceleration;

			}
			else
			{
				if(currentSpeed < 0)
				{
					currentSpeed = -maxSpeed;
				}
				else if(currentSpeed > 0)
				{
					currentSpeed = maxSpeed;
				}
			}

			if(currentSpeed > 0)
			{
				isForwardMoved = true;
			}
			else if(currentSpeed <0)
			{
				isForwardMoved = false;
			}

			playerInputCheck();
		}
      
        transform.position += currentSpeed * transform.forward * time * accelBlock * GameManager.Instance.GameSpeed;



		// ȸ��
		if(yPos != 0)
		{
			currentAngle += yPos * turnSpeed * time;

			if (currentAngle < -180)
			{
				currentAngle += 360;
			}
			else if (currentAngle > 180)
			{
				currentAngle -= 360;
			}


			transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x,
									currentAngle,
									transform.rotation.eulerAngles.z);
		}




		// ������ٵ� ȸ��
		/*if (Ypos != 0)
		{
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


		if (Mathf.Abs(_rigidbody.angularVelocity.y) > 10)
		{
			Debug.Log(_rigidbody.angularVelocity.y);

		}

		currentAngle += _rigidbody.angularVelocity.y;

		if (xPos != 0)
		{
			playerForward = Quaternion.Euler(0f, currentAngle, 0f) * Vector3.forward;

			_rigidbody.velocity += playerForward * xPos * Time.deltaTime * acceleration;


			AccelerationLimit();



		}*/

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

		if(isForwardMoved)
		{
			currentSpeed -= Time.fixedDeltaTime * decelerationLevel;

			if(currentSpeed < 0)
			{
				currentSpeed = 0;
			}
		}
		else
		{
			currentSpeed += Time.fixedDeltaTime * decelerationLevel;

			if (currentSpeed > 0)
			{
				currentSpeed = 0;
			}
		}




		// ������ٵ��
		/*Vector3 currentVelocity = _rigidbody.velocity;
		currentVelocity -= currentVelocity * decelerationLevel * Time.deltaTime;
		_rigidbody.velocity = currentVelocity;*/



	}



	private void ModelRotation()
	{
		angle -= currentSpeed * Time.fixedDeltaTime * 190f * GameManager.Instance.GameSpeed;
		model.transform.localEulerAngles = new Vector3(0,
												angle,
												-90);
	}


	// �÷��̾� �ð� ����
	private void playerDeathTimeCheck()
	{
		if(aliveTime == 0)
		{
			return;
		}

		if(currentIdleTime > aliveTime)
		{
			GameOver();
		}
	}

	private void playerInputCheck()
	{
		currentIdleTime = 0;
	}


	private void BlockCheck()
	{
		RaycastHit hit;

		if(Physics.Raycast(transform.position, Vector3.up * -1, out hit, 1))
		{
			if (hit.collider.gameObject.CompareTag("Slide"))
			{
				accelBlock = 1.5f;
			}
			else
			{
				accelBlock = 1f;
			}
		}
	}


	public void OutCheck()
	{
		if(transform.position.y < -5)
		{
			GameOver();
		}
	}

	public void GameStop()
	{

	}

	public void GameStart()
	{

	}

	public void Respawn()
	{
		// ��ġ �ʱ�ȭ
		transform.position = respawnPosition;

		// �����̼� �ʱ�ȭ
		angle = 0;
		currentAngle = defaultAngle;
		transform.eulerAngles = new Vector3(0, defaultAngle, 90);

		// �ӵ� �ʱ�ȭ
		currentSpeed = 0;

		// ������ٵ� �ʱ�ȭ
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		_rigidbody.angularDrag = 0;
	}



	public void GameClear()
	{
		gameclear.Invoke();

		Debug.Log("���� Ŭ����");
	}


	public void GameOver()
	{
		// �̺�Ʈ ����
		gameover.Invoke();

		Debug.Log("���� ����");
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Trap"))
		{
			GameOver();
		}
		else if(other.CompareTag("WayPoint"))
		{
			GameClear();
		}
	}
}
