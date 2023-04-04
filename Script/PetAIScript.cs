using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PetAIScript : MonoBehaviour
{
    public float speed = 200f;
    public float newxtWaypointDistance = 3f;
    public Transform target;

    Path path;
    int currentWayPoint = 0;

    bool reachedEndOfPath  = false;
    Seeker seeker;
    Rigidbody2D rb;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        //seeker.StartPath(rb.position, target.position, OnPathComplete);
        InvokeRepeating("UpdatePath", 1f, 0.5f);
    }
    void OnPathComplete(Path p)
    {
        if(p.error) return;
        else
        {
            path = p; 
            currentWayPoint = 0;
        }
    }
    void UpdatePath()
    {
        if(seeker.IsDone()) 
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }
    private void Update()
    {
        if(path == null)
        {
            return;
        }
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true; return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint]-rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;
        rb.AddForce(force);
        float distance =Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if(distance < newxtWaypointDistance)
        {
            currentWayPoint++;
        }
        if(force.x >= 0.01)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(force.x <= -0.01)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
