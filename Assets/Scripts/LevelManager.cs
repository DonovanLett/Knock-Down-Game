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
    private int _indexForRandomization;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FirstLevel()
    {
        ShuffleRandomizedLevels(); // Round System Code
        _audioSource.Play();
        _currentLevel = 0;
        _levels[0].SetActive(true);
        //_simulatedPhysics.AddDrumsToPhysicsScene(_levels[0].transform); /// Trajectory
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
        else
        {
            ShuffleRandomizedLevels(_levels[_levels.Length - 1]); // Round System Code;
            _currentLevel = _indexForRandomization; // Round System Code; Originally _currentLevel is set to 0;
        }
        _levels[_currentLevel].SetActive(true);
        //_simulatedPhysics.AddDrumsToPhysicsScene(_levels[_currentLevel].transform); /// Trajectory
        _threshold.GetRoundBottles(_levels[_currentLevel].transform);
    }

    public void OnTimerEnded()
    {
        _levels[_currentLevel].SetActive(false);
        ResetBottlesInLevel(_currentLevel);
        _currentLevel = 0;
    }

    // Round System Code
    ///
    private void ShuffleRandomizedLevels()
    {
        for (int i = _levels.Length - 1; i > _indexForRandomization; i--)
        {
            int randomIndex = Random.Range(_indexForRandomization, i + 1); // inclusive max
            GameObject temp = _levels[i];
            _levels[i] = _levels[randomIndex];
            _levels[randomIndex] = temp;
        }
    }
    ///

    // Round System Code
    ///
    private void ShuffleRandomizedLevels(GameObject exception)
    {
        for (int i = _levels.Length - 1; i > _indexForRandomization; i--)
        {
            int randomIndex = Random.Range(_indexForRandomization, i + 1); // inclusive max
            GameObject temp = _levels[i];
            _levels[i] = _levels[randomIndex];
            _levels[randomIndex] = temp;
        }

        if (_levels[_indexForRandomization] == exception)
        {
            int randomIndex = Random.Range(_indexForRandomization + 1, _levels.Length - 1); // inclusive max
            GameObject temp = _levels[_indexForRandomization];
            _levels[_indexForRandomization] = _levels[randomIndex];
            _levels[randomIndex] = temp;
        }
    }
    ///

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
