using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int _time;

    [SerializeField]
    private int _currentTime;

   // [SerializeField]
    private double _nextTickTime;

    [SerializeField]
    private AudioSource _tickSource;

    [SerializeField]
    private RoundManager _roundManager;

    [SerializeField]
    private LevelManager _levelManager;

    [SerializeField]
    private PointSystem _pointSystem;

    [SerializeField]
    private Threshold _threshold;

    // Start is called before the first frame update
    void Start()
    {
        // StartTimer(); // On Round Start
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        _currentTime = _time;
        //
        Debug.Log(_currentTime);
        double dspTime = AudioSettings.dspTime;
        _nextTickTime = dspTime + 1.0f;
        ScheduleNextTick();
        //
        // ConnectClickWithTimer();

        // StartCoroutine(TimerInAction());
    }

    IEnumerator TimerInAction()
    {
        Debug.Log(_currentTime);
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _currentTime--;
           // _tickSource.Play(); // Timer
            Debug.Log(_currentTime);
        }
        yield return new WaitForSeconds(1.0f);
        _levelManager.OnTimerEnded();
        _pointSystem.OnTimerEnded();
        _threshold.OnTimerEnded();
        _roundManager.EnableFollowingRoundsInput(); // Get rid of this line in the future
    }

    //
    void ScheduleNextTick()
    {
        if (_currentTime <= 0)
        {
            return;
        }
        _tickSource.PlayScheduled(_nextTickTime);

        _nextTickTime += 1.0f; // 1 second intervals
        _currentTime--;
        Debug.Log(_currentTime);

        Invoke(nameof(ScheduleNextTick), 1f);
    }
    //

    /* public void ConnectClickWithTimer()
     {
         _currentTime = _time;
         double dspTime = AudioSettings.dspTime;

         for (int i = 0; i < _currentTime; i++)
         {
             _tickSource.PlayScheduled(dspTime + i + 1);
         }
     } */
}