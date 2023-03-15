using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health;
    public float checkRadius;

    [SerializeField] private LayerMask layer;
    private NavMeshAgent agent;
    private int maxHeaalth;
    private GameObject player;
    private bool pursue;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        maxHeaalth = health;
    }

    void Update()
    {
        CheckPlayer();
        if(pursue)
            Move();
    }
    private void Move()
    {
        agent.SetDestination(player.transform.position);
    }

    private void CheckPlayer()
    {
        Collider[] playerCol = Physics.OverlapSphere(transform.position, checkRadius, layer);
        if(playerCol != null)
        {
            for(int i = 0; i < playerCol.Length; i++)
                player = playerCol[i].gameObject;
            pursue = true;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
