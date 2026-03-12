using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private Transform _turret;

    [SerializeField]
    private float _yRotationSpeed, _zRotationSpeed;

    [SerializeField]
    private float _defaultYRotation, _defaultZRotation;

    [SerializeField]
    private ArticulationReducedSpace _defaultCannonJointPosition, _defaultTurretJointPosition;

    [SerializeField]
    private Projectile _projectile;

    [SerializeField]
    private Vector3 _packageOffset;

    [SerializeField]
    private Vector3 _launchForce;

    [SerializeField]
    private float _force;

    [SerializeField]
    private bool _trajectoryEnabled;

    [SerializeField]
    private AudioClip _audioClip;

    [SerializeField]
    private PlayerInputActions _playerInput;

    [SerializeField]
    private SimulatedPhysics _simulatedPhysics;
    // Start is called before the first frame update
    void Start()
    {
        ArticulationBody body = GetComponent<ArticulationBody>();
        _defaultCannonJointPosition = body.jointPosition;

        ArticulationBody turretBody = _turret.GetComponent<ArticulationBody>();
        _defaultCannonJointPosition = turretBody.jointPosition;

       // _defaultYRotation = transform.rotation.y;
       // _defaultZRotation = _turret.transform.rotation.z;
    }

    public void EnableControl()
    {
        _playerInput.Cannon.Enable();
        _playerInput.Cannon.Fire.performed += Fire;

          _playerInput.Cannon.Trajectory.performed += Trajectory;

      /*  _playerInput.Cannon.Trajectory.started += EnableTrajectory; // Trajectory
        _playerInput.Cannon.Trajectory.canceled += DisableTrajectory; // Trajectory */
    }

    public void DisableControl()
    {
        _playerInput.Cannon.Fire.performed -= Fire;

        _playerInput.Cannon.Trajectory.performed -= Trajectory;

        /* _playerInput.Cannon.Trajectory.started -= EnableTrajectory; // Trajectory
         _playerInput.Cannon.Trajectory.canceled -= DisableTrajectory; // Trajectory*/
        _playerInput.Cannon.Disable();

        if (_trajectoryEnabled) // Trajectory B Code
        {
            _trajectoryEnabled = false;
            _simulatedPhysics.DestroyLine();
        }
    }

    private void OnEnable()
    {
        _playerInput = new PlayerInputActions();
    }

   /* private void EnableTrajectory(UnityEngine.InputSystem.InputAction.CallbackContext context) // Trajectory
    {
        _trajectoryEnabled = true;
    }

    private void DisableTrajectory(UnityEngine.InputSystem.InputAction.CallbackContext context) // Trajectory
    {
        _trajectoryEnabled = false;
        _simulatedPhysics.DestroyLine();
    }*/

     private void Trajectory(UnityEngine.InputSystem.InputAction.CallbackContext context)
     {
         if (_trajectoryEnabled)
         {
             _trajectoryEnabled = false;
             _simulatedPhysics.DestroyLine();
         }
         else
         {
             _trajectoryEnabled = true;
         }
     }

    private void Fire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector3 spawnPosition = _turret.transform.TransformPoint(_packageOffset);
        Projectile projectile = Instantiate(_projectile, spawnPosition, _turret.transform.rotation);
        AudioSource.PlayClipAtPoint(_audioClip, spawnPosition, 1);
        projectile.transform.parent = _turret.transform.root;
        if (projectile != null)
        {
            projectile.Fire(_launchForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_trajectoryEnabled)
        {
            _simulatedPhysics.SimulatedTrajectory(_projectile, _turret.transform.TransformPoint(_packageOffset), _turret.transform.rotation, _launchForce);
        }
    }

    void FixedUpdate()
    {
        VerticalMovement();
        HorizontalMovement();
    }

    private void VerticalMovement()
    {
          ArticulationBody turretBody = _turret.GetComponent<ArticulationBody>();
          var movement = _playerInput.Cannon.Movement.ReadValue<Vector2>();
          if (turretBody != null)
          {
             turretBody.AddRelativeTorque(new Vector3(0, 0, movement.y) * _zRotationSpeed, ForceMode.Force);
          } 

        
      /*  var movement = _playerInput.Cannon.Movement.ReadValue<Vector2>();
        _turret.transform.Rotate(new Vector3(0, movement.y, 0) * Time.deltaTime * _yRotationSpeed); */
    }

    private void HorizontalMovement()
    {
          ArticulationBody body = GetComponent<ArticulationBody>();
          var movement = _playerInput.Cannon.Movement.ReadValue<Vector2>();
          if (body != null)
          {
              body.AddRelativeTorque(new Vector3(0, movement.x, 0) * _yRotationSpeed, ForceMode.Force);
          } 

        
       /* var movement = _playerInput.Cannon.Movement.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, movement.x, 0) * Time.deltaTime * _yRotationSpeed); */
        
    }

    public void ResetRotation()
    {
       /* if (_trajectoryEnabled) // Trajectory B Code
        {
            _trajectoryEnabled = false;
            _simulatedPhysics.DestroyLine();
        }*/

        ArticulationBody body = GetComponent<ArticulationBody>();
        body.jointPosition = _defaultCannonJointPosition;


        ArticulationBody turretBody = _turret.GetComponent<ArticulationBody>();
        turretBody.jointPosition = _defaultTurretJointPosition;
    }
}