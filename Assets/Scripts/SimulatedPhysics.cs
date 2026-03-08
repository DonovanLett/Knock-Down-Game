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

    // Replica Code
    ///
    [SerializeField]
    private List<Rigidbody> _realBodies; // Originally Rigidbodies

    [SerializeField]
    private List<Rigidbody> _simulatedBodies; // Originally Rigidbodies
    ///

    /* [SerializeField]
     private Transform _labParent;*/
    // Start is called before the first frame update
    void Start()
    {
      ///  _labParent = gameObject.transform.parent.root; /// Parent
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

       /* foreach (Transform thing in _labParent)  ///
        {                                        /// For each Object you want being simulated within the Simulated Scene (in this case, all Objects in the _labParent that are tagged "Obstacle")
            if (thing.tag == "Obstacle")          ///
            {
                // Duplicate that object in the exact same spot and angle, but then immediately sets its Renderer to false, making it invisible
                ///
                var simulatedObstacle = Instantiate(thing.gameObject, thing.transform.position, thing.transform.rotation);
                if (simulatedObstacle.GetComponent<Renderer>() != null)
                {
                    simulatedObstacle.GetComponent<Renderer>().enabled = false;
                }
                ///

                // Switch the object over to the Simulated Scene, preventing it from being able to interact with any real-world objects
                ///
                SceneManager.MoveGameObjectToScene(simulatedObstacle, _simulatedScene);
                ///
            }
        }*/
    }


    public void AddDrumsToPhysicsScene(Transform newRound) // Trajectory
    {
        // Replica Code
        ///
        _realBodies.Clear();
        _simulatedBodies.Clear();
        ///


        if (_simulatedScene.GetRootGameObjects().Length != 0)
        {
            foreach (GameObject obj in _simulatedScene.GetRootGameObjects())
            {
                Destroy(obj);
            }
        }

        foreach (Transform thing in newRound)  ///
        {                                        /// For each Object you want being simulated within the Simulated Scene (in this case, all Objects in the newRound that are tagged "Obstacle")
            if (thing.tag == "Drum")          ///
            {
                // Duplicate that object in the exact same spot and angle, but then immediately sets its Renderer to false, making it invisible
                ///
                var simulatedObstacle = Instantiate(thing.gameObject, thing.transform.position, thing.transform.rotation);
                if (simulatedObstacle.GetComponent<Renderer>() != null)
                {
                    simulatedObstacle.GetComponent<Renderer>().enabled = false;

                }

                /// Replica Code
                //
                //_realBodies = new List<Rigidbody>(thing.GetComponentsInChildren<Rigidbody>());
                //_simulatedBodies = new List<Rigidbody>(simulatedObstacle.GetComponentsInChildren<Rigidbody>());
                //

                /// Replica Code
                //
                _realBodies = new List<Rigidbody>();
                _simulatedBodies = new List<Rigidbody>();

                foreach (Rigidbody rb in thing.GetComponentsInChildren<Rigidbody>())
                {
                    if (!rb.isKinematic)
                        _realBodies.Add(rb);
                }

                foreach (Rigidbody rb in simulatedObstacle.GetComponentsInChildren<Rigidbody>())
                {
                    if (!rb.isKinematic)
                        _simulatedBodies.Add(rb);
                }
                //



                MeshRenderer[] renderers = simulatedObstacle.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer r in renderers)
                {
                    r.enabled = false;
                }
                

                SceneManager.MoveGameObjectToScene(simulatedObstacle.gameObject, _simulatedScene);

                
            }
        }
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
        }

        // Once the entire line is set, destroy the simulated projectile to make room for the one next frame
        ///
        Destroy(simulatedObj.gameObject);
        ///
    }

    public void SimulatedTrajectoryWithoutSimulatedScene(Projectile projectile, Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        /// Replica Code
        //
        for (int i = 0; i < _realBodies.Count; i++)
        {
            _simulatedBodies[i].transform.position = _realBodies[i].position;
            _simulatedBodies[i].rotation = _realBodies[i].rotation;
            _simulatedBodies[i].velocity = _realBodies[i].velocity;
            _simulatedBodies[i].angularVelocity = _realBodies[i].angularVelocity;
        }
        //

        // Instantiates the desired projectile within the Scene, but then immediately sets its Renderer to false, making it invisible
        ///
        var simulatedObj = Instantiate(projectile, pos, rot);
        SceneManager.MoveGameObjectToScene(simulatedObj.gameObject, _simulatedScene);
        if (simulatedObj.GetComponent<Renderer>() != null)
        {
            simulatedObj.GetComponent<Renderer>().enabled = false;
        }
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





           /* Collider[] _simulatedColliders = new Collider[_simulatedBodies.Count];

            for (int t = 0; t < _simulatedBodies.Count; t++)
            {
                _simulatedColliders[t] = _simulatedBodies[t].GetComponent<Collider>();
            }

            int hits = _physicsScene.OverlapSphere(simulatedObj.transform.position, 0.325f, _simulatedColliders, ~0, QueryTriggerInteraction.UseGlobal);

            if (hits > 0)
            {
                _line.positionCount = i; // Maybe delete
                break;
            } */

            
            // If line hits any object, have this for each loop return
            if (Physics.CheckSphere(simulatedObj.transform.position, 0.325f)) // Originally, 0.65f // Find a way to make this only effect objects in the _simulatedScene
            {
                _line.positionCount = i; // Maybe delete
                break;
            } 
            
             
        }

        // Once the entire line is set, destroy the simulated projectile to make room for the one next frame
        ///
        Destroy(simulatedObj.gameObject);
        ///
    }

    public void SimulatedTrajectoryWithoutSimulatedScene02(Projectile projectile, Vector3 pos, Quaternion rot, Vector3 velocity)
    {
        // Instantiates the desired projectile within the Scene, but then immediately sets its Renderer to false, making it invisible
        ///
        var simulatedObj = Instantiate(projectile, pos, rot);
        SceneManager.MoveGameObjectToScene(simulatedObj.gameObject, _simulatedScene);
        if (simulatedObj.GetComponent<Renderer>() != null)
        {
            simulatedObj.GetComponent<Renderer>().enabled = false;
        }
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
            if (Physics.CheckSphere(simulatedObj.transform.position, 0.325f)) // Originally, 0.65f // Find a way to make this only effect objects in the _simulatedScene
            {
                _line.positionCount = i; // Maybe delete
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






    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}