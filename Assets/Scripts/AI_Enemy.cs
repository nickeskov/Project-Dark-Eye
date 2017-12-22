using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

class AI_Enemy : MonoBehaviour
{
    public enum AI_State_Machine
    {
        // Hash cods for names of states
        IDLE = 2081823275,
        PATROL = 207038023,
        CHASE = 1463555229,
        ATTACK = 1080829965,
    }

    public AI_State_Machine Current_State = AI_State_Machine.IDLE;
    public float FieldOfView = 180f;

    private int _sightmask;
    //  Animator anim;
    private NavMeshAgent _navigAgent;
  
    private bool _canSeePlayer;
    private Transform _thisTransform;
    private float DisEps = 3f;
    private float ChaseTimeout = 3f;
    private Collider _thisCollider;
    private Transform _playerTransform;
    private float _currentSpeed = 10f;

    private GameObject[] _waypointsGameObjects;
    private Transform[] _waypointsTransforms;

    // Use this for initialization
    void Awake()
    {
        _sightmask = LayerMask.GetMask("Enviroment");
        //   anim = GetComponent<Animator>();
        _navigAgent = GetComponent<NavMeshAgent>();
        _thisTransform = transform;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _thisCollider = GetComponent<SphereCollider>();
        //int p = Animator.StringToHash("Idle");
        //Debug.Log(p.ToString());
        _waypointsGameObjects = GameObject.FindGameObjectsWithTag("WayPoint");
        _waypointsTransforms = (from GameObject go in _waypointsGameObjects select go.transform).ToArray();
        StartCoroutine(State_Idle());
    }

    public IEnumerator State_Idle()
    {
        Current_State = AI_State_Machine.IDLE;
        // anim.SetTrigger((int)AI_State_Machine.IDLE);
        _navigAgent.Stop();
        while (Current_State == AI_State_Machine.IDLE)
        {
            if (_canSeePlayer)
            {
                StartCoroutine(State_Chase());
                yield break;
            }

            if (_canSeePlayer && Vector3.Distance(_thisTransform.position, _playerTransform.position) <= DisEps)
            {
                StartCoroutine(State_Attack());
                yield break;

            }

            float timer = 0f;
            while (timer <= 3f)
            {
                timer += Time.deltaTime;
                yield return null;

            }

            StartCoroutine(State_Patrol());
            yield break;

        }
    }
    public IEnumerator State_Chase()
    {
        Current_State = AI_State_Machine.CHASE;
        //  anim.SetTrigger((int)AI_State_Machine.CHASE);
        while (Current_State == AI_State_Machine.CHASE)
        {
            _navigAgent.SetDestination(_playerTransform.position);
            if (!_canSeePlayer)
            {
                float ElapsedTime = 0f;
                while (true)
                {
                    _navigAgent.Resume();
                    _navigAgent.SetDestination(_playerTransform.position);
                    _navigAgent.speed = 10f;
                    ElapsedTime += Time.deltaTime;
                    //   yield return null;
                    if (ElapsedTime < ChaseTimeout) continue;

                    if (!_canSeePlayer)
                    {
                        _navigAgent.speed = 4f;
                        StartCoroutine(State_Idle());
                        yield break;
                    }
                    else break;
                }
            }
            if (Vector3.Distance(_thisTransform.position, _playerTransform.position) <= DisEps)
            {
                StartCoroutine(State_Attack());
                yield break;
            }
            yield return null;
        }
    }

    public IEnumerator State_Attack()
    {
        Current_State = AI_State_Machine.ATTACK;
        //  anim.SetTrigger((int)AI_State_Machine.ATTACK);
        _navigAgent.Stop();
        float EllapsedTime = 0f;
        while (Current_State == AI_State_Machine.ATTACK)
        {
            if (!_canSeePlayer || Vector3.Distance(_thisTransform.position, _playerTransform.position) > DisEps)
            {
                _navigAgent.Resume();
                StartCoroutine(State_Chase());
                yield break;
            }
            EllapsedTime += Time.deltaTime;
            if (EllapsedTime >= 1.5f)
            {
                //  anim.SetTrigger((int)AI_State_Machine.ATTACK);
                EllapsedTime = 0f;
                yield return null;
            }

            yield return null;
        }

    }

    public void OnIdleStateCompleted()
    {
        StopAllCoroutines();
        StartCoroutine(State_Patrol());
    }

    public IEnumerator State_Patrol()
    {
        Current_State = AI_State_Machine.PATROL;
        // anim.SetTrigger((int)AI_State_Machine.PATROL);

        Transform randomDest = _waypointsTransforms[Random.Range(0, _waypointsTransforms.Length)];
        _navigAgent.SetDestination(randomDest.position);
        _navigAgent.Resume();

        while (Current_State == AI_State_Machine.PATROL)
        {
            if (_canSeePlayer)
            {
                StartCoroutine(State_Chase());
                yield break;
            }
            if (Vector3.Distance(_thisTransform.position, randomDest.position) <= DisEps)
            {
                StartCoroutine(State_Idle());
                yield break;
            }

            yield return null;
        }
    }

    void Update()
    {
        Debug.Log(Current_State.ToString());
        _canSeePlayer = false;

        if (!_thisCollider.bounds.Contains(_playerTransform.position)) return;

        _canSeePlayer = HaveLineOfSight(_playerTransform);
    }

    private bool HaveLineOfSight(Transform playerTransform)
    {
        float angle = Mathf.Abs(Vector3.Angle(_thisTransform.forward, (_thisTransform.position - playerTransform.position).normalized));

        if (angle >= FieldOfView) return false;

        return !Physics.Linecast(_thisTransform.position, playerTransform.position, _sightmask);
    }
}
