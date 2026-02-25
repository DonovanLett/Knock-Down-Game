using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int _time;

    [SerializeField]
    private int _currentTime;

    [SerializeField]
    private RoundManager _roundManager;

    [SerializeField]
    private PointSystem _pointSystem;

    [SerializeField]
    private Threshold _threshold;

    // Start is called before the first frame update
    void Start()
    {
        StartTimer(); // On Round Start
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        _currentTime = _time;
        StartCoroutine(TimerInAction());
    }

    IEnumerator TimerInAction()
    {
        Debug.Log(_currentTime);
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _currentTime--;
            Debug.Log(_currentTime);
        }
        yield return new WaitForSeconds(1.0f);
        _roundManager.OnTimerEnded();
        _pointSystem.OnTimerEnded();
        _threshold.OnTimerEnded();
    }
}