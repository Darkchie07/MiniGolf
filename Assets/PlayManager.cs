using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    private const string saveLevel = "SAVELEVEL";
    [SerializeField] private AudioClip goal;
    [SerializeField] private BallController ballController;
    [SerializeField] private CameraController camController;
    [SerializeField] private GameObject finishWindow;
    [SerializeField] private TMP_Text finishText;
    [SerializeField] private TMP_Text shootCountText;
    private bool isBallOutside;
    private bool isBallTeleporting;
    private bool isGoal;
    private Vector3 lastBallPosition;
    public float idLevel;

    private void OnEnable()
    {
        ballController.onBallShooted.AddListener(UpdateShootCount);
    }

    private void OnDisable()
    {
        ballController.onBallShooted.RemoveListener(UpdateShootCount);
    }

    void Update()
    {
        if (ballController.ShootingMode)
        {
            lastBallPosition = ballController.transform.position;
        }
        var InputActive = Input.GetMouseButton(0) 
                          && ballController.IsMove() == false 
                          && ballController.ShootingMode == false
                          && isBallOutside == false;
        
        camController.SetInputActive(InputActive);
    }

    public void OnBallGoalEnter()
    {
        isGoal = true;
        ballController.enabled = false;
        finishWindow.gameObject.SetActive(true);
        finishText.text = "Finished!!\n" + "Shoot Count : " + ballController.ShootCount;
        var lastLevel = PlayerPrefs.GetInt(saveLevel);
        if (lastLevel <= idLevel)
        {
            lastLevel += 1;
            PlayerPrefs.SetInt(saveLevel, lastLevel);
        }
        SoundManager.Instance.playSFX(goal);
    }

    public void OnBallOutside()
    {
        if (isGoal)
            return;
        
        if(isBallTeleporting == false)
            Invoke("TeleportBallLastPosition", 3);
        ballController.enabled = false;
        isBallOutside = true;
        isBallTeleporting = false;
    }

    public void TeleportBallLastPosition()
    {
        TeleportBall(lastBallPosition);
    }

    public void TeleportBall(Vector3 targetposition)
    {
        var rb = ballController.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballController.transform.position = targetposition;
        rb.isKinematic = false;
        
        ballController.enabled = true;
        isBallOutside = false;
        isBallOutside = false;
    }

    public void UpdateShootCount(int shootCount)
    {
        shootCountText.text = shootCount.ToString();
    }
}
