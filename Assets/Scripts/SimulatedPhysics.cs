using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;

public class SimulatedPhysics : MonoBehaviour
{
    private Scene _simulatedScene;
    private PhysicsScene _physicsScene;

    // Start is called before the first frame update
    void Start()
    {
        _line.positionCount = 0;
        CreateSimulatedPhysicsScene();
    }

    private void CreateSimulatedPhysicsScene() // Usually called only once, when the Scene first starts
    {
        // Create a new Scene within your Project Hierarchy
        ///
        _simulatedScene = SceneManager.CreateScene("SimulatedPhysics", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        ///

        // Gets a reference to the Physics within that Scene
        ///
        _physicsScene = _simulatedScene.GetPhysicsScene();
        ///
    }
   
    [SerializeField]
    private LineRenderer _line;
    [SerializeField]
    private int _maxPhysicsIterations;

    // Called every frame from your Launcher Script, taking its parameters from there as well.
    public void SimulatedTrajectory(Projectile projectile, Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        // Instantiates the desired projectile within the Scene, but then immediately sets its Renderer to false, making it invisible
        ///
        var simulatedObj = Instantiate(projectile, pos, rot);
        if (simulatedObj.GetComponent<Renderer>() != null)
        {
            simulatedObj.GetComponent<Renderer>().enabled = false;
        }
        ///

        // Switch the projectile over to the Simulated Scene, preventing it from being able to interact with any real-world objects
        ///
        SceneManager.MoveGameObjectToScene(simulatedObj.gameObject, _simulatedScene);
        ///

        // Fire the projectile off in the desired direction
        ///
        simulatedObj.Fire(velocity);
        ///

        // Set the number of vertices that _line has to equal _maxPhysicsIterations
        _line.positionCount = _maxPhysicsIterations;

        // For each vertex in _line
        for (int i = 0; i < _maxPhysicsIterations; i++)
        {
            // Bump the Physics Scene ahead to where everything will be in one Time.fixedDeltaTime frame
            _physicsScene.Simulate(Time.fixedDeltaTime);

            // ...and set the current _line vertex to where the projectile is in that exact moment
            _line.SetPosition(i, simulatedObj.transform.position);

            // If line hits any object, have this for each loop return
            if (Physics.CheckSphere(simulatedObj.transform.position, 0.325f))
            {
                _line.positionCount = i;
                break;
            }

        }

        // Once the entire line is set, destroy the simulated projectile to make room for the one next frame
        ///
        Destroy(simulatedObj.gameObject);
        ///
    }

    public void DestroyLine() // Trajectory
    {
        _line.positionCount = 0;
    }

    
}