using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _levels;

    [SerializeField]
    private int _currentLevel;

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
        //FirstLevel(); // On Round Start
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FirstLevel()
    {
        _audioSource.Play();
        _currentLevel = 0;
        _levels[0].SetActive(true);
        //  _simulatedPhysics.AddDrumsToPhysicsScene(_rounds[0].transform); /// Trajectory
        _threshold.GetRoundBottles(_levels[0].transform);
    }

    public void NextLevel()
    {
        _audioSource.Play();
        _levels[_currentLevel].SetActive(false);
        ResetBottlesInLevel(_currentLevel);
        if (_currentLevel < _levels.Length - 1)
        {
            _currentLevel++;
        }
        else /// Delete in a second
        {
            _currentLevel = 0;
        }
        _levels[_currentLevel].SetActive(true);
        //_simulatedPhysics.AddDrumsToPhysicsScene(_rounds[_currentRound].transform); /// Trajectory
        _threshold.GetRoundBottles(_levels[_currentLevel].transform);
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
        _levels[_currentLevel].SetActive(false);
        ResetBottlesInLevel(_currentLevel);
        _currentLevel = 0;
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

            if (t.transform.IsChildOf(_levels[level].transform))
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
