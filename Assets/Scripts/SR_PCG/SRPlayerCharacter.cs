using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraTrackingType
{
	Chunk20x18,
	Horizontal,
	None
}

public class SRPlayerCharacter : MonoBehaviour
{
	private Animator _anim;
	private Rigidbody2D _body;
	[SerializeField]
	private SRFoot _foot;

	private const float JumpInitSpeed = 11.0f;
	private const float WalkSpeed = 5.0f;
	private const float CameraSpeed = 7.0f;

	private bool _jumpButton = false;
	private bool _grounded = false;
	private bool _facingRight = true;
	[SerializeField]
	CameraTrackingType _cameraTrackingType = CameraTrackingType.Chunk20x18;
	private Camera _mainCamera;
	// Use this for initialization
	void Start ()
	{
		_mainCamera = Camera.main;
		_anim = GetComponent<Animator>();
		_body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		_body.velocity = _body.velocity.withX(Input.GetAxisRaw("Horizontal") * WalkSpeed);
		if (_jumpButton && _grounded)
		{
			Jump();
		}
		_jumpButton = false;
	}

	void Jump()
	{
		_body.velocity = _body.velocity.withY(JumpInitSpeed);
	}

	void LateUpdate()
	{
		switch (_cameraTrackingType)
		{
			case CameraTrackingType.Chunk20x18:
				{
					Vector3 newCameraPosition = new Vector3(
						(int)transform.position.x / (SRChunk.SizeX * SRChunk.TileSize),
						(int)transform.position.y / (SRChunk.SizeY * SRChunk.TileSize), -10);
					Vector3 deltaPos = newCameraPosition - _mainCamera.transform.position;
					if (deltaPos.magnitude > 1.0f)
					{
						_mainCamera.transform.position += deltaPos.normalized * CameraSpeed * Time.deltaTime;
					}
				}
				break;
			case CameraTrackingType.Horizontal:
				{
					Vector3 newCameraPosition = transform.position.withY(0).withZ(-10);
					Vector3 deltaPos = newCameraPosition - _mainCamera.transform.position;
					
					_mainCamera.transform.position += deltaPos*CameraSpeed*Time.deltaTime;
					
				}
				break;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		_anim.SetFloat("horizontal", _body.velocity.x);
		_grounded = _foot.Contact > 0;
		_anim.SetBool("grounded", _grounded);

		if (Input.GetButtonDown("Jump"))
		{
			_jumpButton = true;
		}

		if (_facingRight && _body.velocity.x < 0.0f)
		{
			Flip();
		}

		if (!_facingRight && _body.velocity.x > 0.0f)
		{
			Flip();
		}

		
	}
	void Flip()
	{
		//Debug.Log("FLIP");
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		


	}
}
