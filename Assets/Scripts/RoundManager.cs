using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private LevelManager _levelManager;

    [SerializeField]
    private Cannon _cannon;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private PlayerInputActions _playerInput;

    [SerializeField]
    private bool _firstRound;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.FirstRound.Enable();
        _playerInput.FirstRound.BeginRound.performed += BeginRound;
    }

    public void EnableFollowingRoundsInput()
    {
        _playerInput.FollowingRounds.Enable();
        _playerInput.FollowingRounds.BeginRound.performed += BeginRound;
        _cannon.DisableControl(); // Get rid of this line in the future
    }

    private void BeginRound(InputAction.CallbackContext obj)
    {
        if (_firstRound == false)
        {
            _cannon.ResetRotation();
            DisableFollowingRoundsInput();
        }
        else
        {
            _firstRound = false; // Get rid of this line in the future
            DisableFirstRoundInput();
        }
        _timer.StartTimer();
        _levelManager.FirstLevel();
        _cannon.EnableControl();
    }

    private void DisableFirstRoundInput()
    {
        _playerInput.FirstRound.BeginRound.performed -= BeginRound;
        _playerInput.FirstRound.Disable();
    }

    private void DisableFollowingRoundsInput()
    {
        _playerInput.FollowingRounds.BeginRound.performed -= BeginRound;
        _playerInput.FollowingRounds.Disable();
    }
}