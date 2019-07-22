using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActor : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LogicManager Manager { get; set; }

    private Transform position;
    private Animator animator;
    private SpriteRenderer renderers;
    private float inverseMoveTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        position = GetComponent<Transform>();
        renderers = GetComponent<SpriteRenderer>();
        
        inverseMoveTime = 1f / moveTime;
    }

    public void Move(int x, int y)
    {
        DebugLogger.Log("move character [x: {0}, y: {1}]", x, y);
        Vector2 end = (new Vector2(x, y));
        animator.SetBool("run", true);
        if(x > position.position.x)
        {
            renderers.flipX = false;
        }
        else if (x < position.position.x)
        {
            renderers.flipX = true;

        }
        StartCoroutine(SmoothMovement(end));
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainDistance = (transform.position - end).sqrMagnitude;

        while(sqrRemainDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(position.position, end, inverseMoveTime * Time.deltaTime);
            position.position = newPosition;

            sqrRemainDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
        animator.SetBool("run", false);
        Manager.ReturnCommand();
    }





}
