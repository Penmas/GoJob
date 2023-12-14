using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private bool isTitle;

	[Space(10)]
	[Header("�ɷ�ġ")]
	[SerializeField] private Vector3 respawnPosition;
	[SerializeField] private float maxSpeed;                                // �÷��̾� �ְ� �ӵ�
	[SerializeField] private float acceleration;							// ����
	[SerializeField] private float decelerationLevel;                       // ����
	[SerializeField] private float turnSpeed;
	[SerializeField] private float aliveTime;

	[Space(10)]
	[Header("�𵨸�")]
	[SerializeField] private GameObject model;                              // �𵨸�
	[SerializeField] private GameObject leftModel;
	[SerializeField] private GameObject rightModel;
	[SerializeField] private Collider collder;

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
	private StageFunction stageFunction;

	// ��ġ
	private float defaultAngle;
	private Vector3 leftAngleDefault;
	private Vector3 rightAngleDefault;
	private float angle;					// �𵨸��� ������ ����
	private float accelBlock;				// ���� ��� �ӵ�


	private bool isForwardMoved;            //������ �����̴��� �Ǵ�
	private bool isGameOver;

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
		stageFunction = FindObjectOfType<StageFunction>();
		defaultAngle = transform.eulerAngles.y;
		currentAngle = defaultAngle;
		accelBlock = 1;


		leftAngleDefault = leftModel.transform.localEulerAngles;
		rightAngleDefault = rightModel.transform.localEulerAngles;

	}


	private void Update()
	{
		if(!isTitle)
		{

			if (stageFunction.UIOn)
			{
				return;
			}
		}


		if(isGameOver)
		{
			return;
		}

		// �ð�
		currentIdleTime += Time.deltaTime * GameManager.Instance.GameSpeed;

		playerDeathTimeCheck();
		
		//���� �� üũ
		BlockCheck();

		// ���� ��ġ üũ
		OutCheck();
	}

	private void FixedUpdate()
	{
		if (!isTitle)
		{

			if (stageFunction.UIOn)
			{
				return;
			}
		}

		if (isGameOver)
		{
			return;
		}

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
			// �ӷ�����
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
			currentAngle += yPos * turnSpeed * time * GameManager.Instance.GameSpeed;

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



	// ȸ��
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


	public void Respawn()
	{

		// �𵨸� �ʱ�ȭ
		collder.enabled = true;
		leftModel.transform.localPosition = new Vector3(0, -0.25f, 0);
		leftModel.transform.localEulerAngles = leftAngleDefault;
		rightModel.transform.localPosition = new Vector3(0, -0.25f, 0);
		rightModel.transform.localEulerAngles = rightAngleDefault;



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

		// ������ �ʱ�ȭ
		currentIdleTime = 0;



		// ��ġ �ʱ�ȭ
		transform.position = respawnPosition;
		isGameOver = false;

	}




	public void GameOverPlayerAnimation()
	{
		
		StartCoroutine("GameOverPlayerAnimationCoroutine");	
	}


	public IEnumerator GameOverPlayerAnimationCoroutine()
	{
		float time = 0;
		collder.enabled = false;

		
		while (true)
		{
			time += Time.deltaTime;

			if (time > 1.5f)
			{
				break;
			}
			leftModel.transform.localPosition += Vector3.right * Time.deltaTime * -2f;
			leftModel.transform.localEulerAngles += Vector3.up * Time.deltaTime * 50f;
			rightModel.transform.localPosition += Vector3.right * Time.deltaTime * 2f;
			rightModel.transform.localEulerAngles += Vector3.up * Time.deltaTime * -1 * 50f;
			yield return null;
		}



		Respawn();
	}




	public void GameClear()
	{


		gameclear.Invoke();

		Debug.Log("���� Ŭ����");
	}


	public void GameOver()
	{

		if (isGameOver)
		{
			return;
		}

		// �̺�Ʈ ����
		gameover.Invoke();
		isGameOver = true;
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
