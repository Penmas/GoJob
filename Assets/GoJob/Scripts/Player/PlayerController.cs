using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[Header("능력치")]
	[SerializeField] private Vector3 respawnPosition;
	[SerializeField] private float maxSpeed;                                // 플레이어 최고 속도
	[SerializeField] private float acceleration;							// 가속
	[SerializeField] private float decelerationLevel;                       // 감속
	[SerializeField] private float turnSpeed;
	[SerializeField] private float aliveTime;

	[Space(10)]
	[Header("모델링")]
	[SerializeField] private GameObject model;								// 모델링

	[Space(10)]
	[Header("현재 수치")]
	[SerializeField] private float currentSpeed;							// 현재 속도
	[SerializeField] private float currentAngle;							// 현재 각도
	[SerializeField] private float currentIdleTime;                         // 움직이지 않은 시간

	[Space(10)]
	[Header("이벤트")]
	[SerializeField] private UnityEvent gameclear;
	[SerializeField] private UnityEvent gameover;

	// 컴포넌트
	private Rigidbody _rigidbody;


	// 수치
	private float defaultAngle;
	private float angle;					// 모델링이 움직인 각도
	private float accelBlock;				// 가속 블록 속도


	private bool isForwardMoved;			//앞으로 움직이는지 판단


	// 프로퍼티 선언
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
		// 시간
		currentIdleTime += Time.deltaTime;

		playerDeathTimeCheck();
		
		//현재 블럭 체크
		BlockCheck();

		// 현재 위치 체크
		OutCheck();
	}

	private void FixedUpdate()
	{
		// 플레이어 움직임
		PlayerMove();

		// 플레이어 모델링 회전
		ModelRotation();

		// 플레이어 자동 감속
		PlayerSpeedDeceleration();

	}




	// 플레이어 움직이기
	private void PlayerMove()
	{
		// 앞뒤 이동
		float xPos = Input.GetAxis("Vertical");
		float yPos = Input.GetAxis("Horizontal");



		float time = Time.fixedDeltaTime;
		//float time = Time.deltaTime;

        // 이동
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



		// 회전
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




		// 리지드바디 회전
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

	// 플레이어 속력 제한
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

	// 플레이어 감속
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




		// 리지드바디용
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


	// 플레이어 시간 제한
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
		// 위치 초기화
		transform.position = respawnPosition;

		// 로테이션 초기화
		angle = 0;
		currentAngle = defaultAngle;
		transform.eulerAngles = new Vector3(0, defaultAngle, 90);

		// 속도 초기화
		currentSpeed = 0;

		// 리지드바디 초기화
		_rigidbody.velocity = Vector3.zero;
		_rigidbody.angularVelocity = Vector3.zero;
		_rigidbody.angularDrag = 0;
	}



	public void GameClear()
	{
		gameclear.Invoke();

		Debug.Log("게임 클리어");
	}


	public void GameOver()
	{
		// 이벤트 실행
		gameover.Invoke();

		Debug.Log("게임 오버");
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
