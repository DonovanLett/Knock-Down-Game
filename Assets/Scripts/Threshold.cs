using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threshold : MonoBehaviour
{
    [SerializeField]
    private RoundManager _roundManager;

     [SerializeField] 
     private PointSystem _pointSystem; 

    [SerializeField]
    private List<GameObject> _bottles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetRoundBottles(Transform round)
    {
        foreach(Transform child in round)
        {
            if(child.tag == "Bottle")
            {
                _bottles.Add(child.gameObject);
            }
            else if (child.childCount != 0)
            {
                GetRoundBottles(child);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_bottles.Contains(other.gameObject))
        {
            _bottles.Remove(other.gameObject);
            _pointSystem.AddPoints();
            ///
            /// Possibly set other to false
            ///
            if (_bottles.Count <= 0)
            {
                _roundManager.NextRound();
            }
        }
    }

    public void OnTimerEnded()
    {
        _bottles.Clear();
    }
}