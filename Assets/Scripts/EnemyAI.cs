using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Animator animator;
    
    
    //Patroling
    public Vector3 walkPoint;
    bool _walkPointSet;
    public float walkPointRange;
    
    //Attacking
    public float timeBetweenAttacks;
    bool _alreadyAttacked;
    
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    [SerializeField] public AudioClip hitSfx;
    [SerializeField] public AudioClip deathSfx;
    //[SerializeField] public AudioClip walkingSfx;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //PlaySFX(walkingSfx);
    }

    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if(playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void Patroling()
    {
        if (!_walkPointSet) SearchWalkPoint();
        if(_walkPointSet)
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            _walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    
    void AttackPlayer()
    {
        //Make sure enemy doesnt move
        agent.SetDestination(transform.position);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // ignore vertical difference
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (!_alreadyAttacked)
        {
            //Attack code here
            animator.SetBool("Attacking", true);
            print("we attacking");
            //PlaySFX(hitSfx);
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
    }

    void ResetAttack()
    {
        _alreadyAttacked = false;
        animator.SetBool("Attacking", false);
        print(animator.GetBool("Attacking"));
        
    }
    
    void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    void OnDestroy()
    {
        //PlaySFX(deathSfx);
    }
}
