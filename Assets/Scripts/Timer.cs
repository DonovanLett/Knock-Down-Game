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
    private int _warningTime, _hazardTime, _panicTime;

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
       // _tickSource.PlayScheduled(AudioSettings.dspTime + 2); // Delete
        // StartTimer(); // On Round Start
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTimer()
    {
        _currentTime = _time;
        StartCoroutine(Countdown());
    }

    /*
    IEnumerator TimerInAction()
    {
        Debug.Log(_currentTime);
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _currentTime--;
           _tickSource.Play(); // Timer
            Debug.Log(_currentTime);
        }
        yield return new WaitForSeconds(1.0f);
        _levelManager.OnTimerEnded();
        _pointSystem.OnTimerEnded();
        _threshold.OnTimerEnded();
        _roundManager.EnableFollowingRoundsInput(); // Get rid of this line in the future
    }
    */

    IEnumerator Countdown()
    {
        Debug.Log(_currentTime);
        while (_currentTime > 0)
        {
            if(_currentTime > _warningTime)
            {
                yield return new WaitForSeconds(1.0f);
            }
            else if(_currentTime <= _panicTime)
            {
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.125f);
            }
            else if (_currentTime <= _hazardTime)
            {
                yield return new WaitForSeconds(0.25f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.25f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.25f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.25f);
            }
            else if (_currentTime <= _warningTime)
            {
                yield return new WaitForSeconds(0.5f);
                _tickSource.PlayOneShot(_tickSource.clip);
                yield return new WaitForSeconds(0.5f);
            }
            _currentTime--;
            Debug.Log(_currentTime);
            if (_currentTime > 0)
            {
                _tickSource.PlayOneShot(_tickSource.clip); // Timer
            }
            else
            {
                OnTimerEnded();
            }
        }
    }

    private void OnTimerEnded()
    {
        _levelManager.OnTimerEnded();
        _pointSystem.OnTimerEnded();
        _threshold.OnTimerEnded();
        _roundManager.EnableFollowingRoundsInput();
    }
}