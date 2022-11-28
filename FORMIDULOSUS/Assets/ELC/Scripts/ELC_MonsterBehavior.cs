using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ELC_MonsterBehavior : MonoBehaviour
{
    enum MonsterState
    {
        PATROL,
        POURSUE,
        SENTINEL,
        SEARCH
    }

    enum SoundDetectionRange
    {
        LOW = 5,
        MEDIUM = 20,
        LARGE = 40
    }

    private Dictionary<MonsterState, SoundDetectionRange> StateSoundDetectRange =
        new Dictionary<MonsterState, SoundDetectionRange>();
    [SerializeField]
    private MonsterState currentState = MonsterState.PATROL;
    [SerializeField]
    private LayerMask playerMask;

    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private float angularVision;
    private Transform _playerTransform;
    private Vector3 _targetPos;
    private bool _reachTargetPos;
    private float _soundDetectionRange;
    private float _sentinelTime;
    
    
    void Start()
    {
        StateSoundDetectRange.Add(MonsterState.PATROL, SoundDetectionRange.MEDIUM);
        StateSoundDetectRange.Add(MonsterState.SEARCH, SoundDetectionRange.LOW);
        StateSoundDetectRange.Add(MonsterState.POURSUE, SoundDetectionRange.LOW);
        StateSoundDetectRange.Add(MonsterState.SENTINEL, SoundDetectionRange.LARGE);
        
        if(!navAgent)navAgent = GetComponent<NavMeshAgent>();
        SetRandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        //Detect player by vision
        _playerTransform = VisionDetection();

        if (_playerTransform)
        {
            currentState = MonsterState.POURSUE;
        }
        else if (currentState == MonsterState.POURSUE)
        {
            _sentinelTime = 0;
            currentState = MonsterState.SENTINEL;
        }
        
        //Detect & stop if target has been reached
        _reachTargetPos = Vector3.Distance(transform.position, _targetPos) < 1;
        navAgent.isStopped = _reachTargetPos;

        switch (currentState)
        {
            case MonsterState.PATROL :
                if(_reachTargetPos) SetRandomTarget();
                break;
            case MonsterState.SEARCH :
                break;
            case MonsterState.POURSUE :
                _targetPos = _playerTransform.position;
                break;
            case MonsterState.SENTINEL :
                if (_sentinelTime < 10) _sentinelTime += Time.deltaTime;
                else currentState = MonsterState.PATROL;
                break;
        }

        _soundDetectionRange = (float)StateSoundDetectRange[currentState];
        navAgent.destination = _targetPos;
    }

    private void SetRandomTarget()
    {
        _targetPos = transform.position + new Vector3(Random.Range(-10, 10), 0,  Random.Range(-10, 10));
    }
    

    /*private Transform SoundDetection()
    {
        
    }*/

    private Transform VisionDetection()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 20, transform.forward, 20, playerMask);
        

        if (hit.Length > 0)
        {
            float angle = Vector3.Angle(hit[0].collider.gameObject.transform.position - transform.position, transform.forward);
            if(angle < angularVision ) return hit[0].transform;
        }
        
        return null;
    }
    
    
}
