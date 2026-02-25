using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField]
    private int _maxPoints;

    [SerializeField]
    private int _currentPoints;

    [SerializeField]
    private int _pointsForEachBottle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints()
    {
        _currentPoints += _pointsForEachBottle;
    }

    public void OnTimerEnded()
    {
        if(_currentPoints > _maxPoints)
        {
            _maxPoints = _currentPoints;
        }
        _currentPoints = 0;
    }
}