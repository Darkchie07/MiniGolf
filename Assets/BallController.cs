using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;
using OpenCover.Framework.Model;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AudioClip hitBall;
    [SerializeField] private AudioClip hitWall;
    [SerializeField] private Collider col;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force;

    [SerializeField] private Transform aimWorld;
    [SerializeField] private LineRenderer aimLine;
    private bool shoot;
    private bool shootingMode;
    private float forceFactor;
    private Ray ray;
    private Plane plane;
    private Vector3 forceDirection;
    private int shootCount;

    public bool ShootingMode
    {
        get => shootingMode;
        set => shootingMode = value;
    }

    public int ShootCount
    {
        get => shootCount;
        set => shootCount = value;
    }

    public UnityEvent<int> onBallShooted = new UnityEvent<int>();

    private void Update()
    {
        if (ShootingMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                aimWorld.gameObject.SetActive(true);
                plane = new Plane(Vector3.up, this.transform.position);
            }else if (Input.GetMouseButton(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                plane.Raycast(ray, out var distance);
                forceDirection = (this.transform.position - ray.GetPoint(distance)).normalized;
                
                //force factor
                var mouseViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                var ballViewportPos = Camera.main.WorldToViewportPoint(this.transform.position);
                var pointerDirection = ballViewportPos - mouseViewportPos;
                pointerDirection.z = 0;
                pointerDirection.z *= Camera.main.aspect;
                pointerDirection.z = Mathf.Clamp(pointerDirection.z, -0.5f, 0.5f);
                forceFactor = pointerDirection.magnitude * 2;
                
                //force direction
                aimWorld.transform.position = this.transform.position;
                aimWorld.forward = forceDirection;
                aimWorld.localScale = new Vector3(1, 1, 0.5f + forceFactor);

                var ballScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
                var mouseScreenPos = Input.mousePosition;
                ballScreenPos.z = 1;
                mouseScreenPos.z = 1;
                var positions = new Vector3[]{Camera.main.ScreenToWorldPoint(ballScreenPos), Camera.main.ScreenToWorldPoint(mouseScreenPos)};
                aimLine.SetPositions(positions);
                aimLine.endColor = Color.Lerp(Color.blue, Color.red, forceFactor);

            }else if (Input.GetMouseButtonUp(0))
            {
                shoot = true;
                shootingMode = false;
                aimWorld.gameObject.SetActive(false);
                SoundManager.Instance.playSFX(hitBall);
            }
        }
    }

    private void FixedUpdate()
    {
        if (shoot)
        {
            shoot = false;
            ShootCount += 1;
            onBallShooted.Invoke(shootCount);
            rb.AddForce(forceDirection * force * forceFactor, ForceMode.Impulse);
        }

        if (rb.velocity.sqrMagnitude < 0.01f && rb.velocity.sqrMagnitude > 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public bool IsMove()
    {
        return rb.velocity != Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.IsMove())
        {
            return;
        }
        ShootingMode = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.Instance.playSFX(hitWall);
        }
    }
}
