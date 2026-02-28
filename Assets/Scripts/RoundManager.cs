using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _rounds;

    [SerializeField]
    private int _currentRound;

    [SerializeField]
    private bool _roundInProgress; // _isRoundActive

    [SerializeField]
    private Threshold _threshold;

    [SerializeField]
    private SimulatedPhysics _simulatedPhysics; // Trajectory

    [SerializeField]
    private AudioSource _audioSource;

    private Dictionary<Transform, TransformData> initialStates =
        new Dictionary<Transform, TransformData>();

    private struct TransformData
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        Transform[] allObjects = Resources.FindObjectsOfTypeAll<Transform>();

        foreach (Transform obj in allObjects)
        {
            if (obj.CompareTag("Bottle"))
            {
                initialStates[obj] = new TransformData
                {
                    localPosition = obj.localPosition,
                    localRotation = obj.localRotation,
                    localScale = obj.localScale
                };
            }
        }
       // _audioSource = GetComponent<AudioSource>();
        FirstRound(); // On Round Start
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirstRound()
    {
        _audioSource.Play();
        _currentRound = 0;
        _rounds[0].SetActive(true);
      //  _simulatedPhysics.AddDrumsToPhysicsScene(_rounds[0].transform); /// Trajectory
        _threshold.GetRoundBottles(_rounds[0].transform);
    }

    public void NextRound()
    {
        _audioSource.Play();
        _rounds[_currentRound].SetActive(false);
        ResetBottlesInLevel(_currentRound);
        if (_currentRound < _rounds.Length - 1)
        {
            _currentRound++;
        }
        else /// Delete in a second
        {
            _currentRound = 0;
        }
        _rounds[_currentRound].SetActive(true);
        //_simulatedPhysics.AddDrumsToPhysicsScene(_rounds[_currentRound].transform); /// Trajectory
        _threshold.GetRoundBottles(_rounds[_currentRound].transform);
    }

   /* public void NextRound() //ORIGINAL
    {
        if (_currentRound < _rounds.Length - 1)
        {
            _rounds[_currentRound].SetActive(false);
            ResetBottlesInLevel(_currentRound);
            _currentRound++;
            _rounds[_currentRound].SetActive(true);
            _threshold.GetRoundBottles(_rounds[_currentRound].transform);
        }
        else /// Delete in a second
        {
            _rounds[_currentRound].SetActive(false);
            ResetBottlesInLevel(_currentRound);
            _currentRound = 0;
            _rounds[_currentRound].SetActive(true);
            _threshold.GetRoundBottles(_rounds[_currentRound].transform);
        }
    } */

    public void OnTimerEnded()
    {
        _roundInProgress = false;
        _rounds[_currentRound].SetActive(false);
        ResetBottlesInLevel(_currentRound);
        _currentRound = 0;
    }

  /*  public void ResetBottles()
    {
        foreach (var pair in initialStates)
        {
            Transform t = pair.Key;
            TransformData data = pair.Value;

            t.localPosition = data.localPosition;
            t.localRotation = data.localRotation;
            t.localScale = data.localScale;
        }
    } */

    public void ResetBottlesInLevel(int level)
    {
        foreach (var pair in initialStates)
        {
            Transform t = pair.Key;

            if (t.transform.IsChildOf(_rounds[level].transform))
            {
                TransformData data = pair.Value;

                t.localPosition = data.localPosition;
                t.localRotation = data.localRotation;
                t.localScale = data.localScale;

                Rigidbody rb = t.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                t.gameObject.SetActive(true);
            }
        }
    }
}