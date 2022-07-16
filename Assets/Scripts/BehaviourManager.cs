using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourManager : MonoBehaviour
{
    public static BehaviourManager i;
    public bool enableTime = true;
    public float tickDur = 0.5f;
    float timer;
    public float slerpCentre = 0.25f;
    public Actor[] actorSpawns;
    public List<Actor> currentActors = new List<Actor>();
    private void Awake()
    {
        i = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnActor(actorSpawns[Random.Range(0, actorSpawns.Length)]);

        if (enableTime)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = tickDur;
                RunTick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RunTick();
        }
    }
    void RunTick()
    {
        foreach (Actor actor in currentActors)
        {
            foreach (BehaviourComponent behaviour in actor.behaviours)
            {
                if (behaviour.OnTick())
                    behaviour.ticks = 0;
            }
            actor.age++;
        }
    }
    public void SpawnActor(Actor actorToSpawn)
    {
        Actor spawned = Instantiate(actorToSpawn, transform.position, Quaternion.identity);
        currentActors.Add(spawned);
    }
}