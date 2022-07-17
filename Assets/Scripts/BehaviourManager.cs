using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BehaviourManager : MonoBehaviour
{
    public static BehaviourManager i;
    public bool enableTime = true;
    public float tickDur = 0.5f;
    float timer;
    public float slerpCentre = 0.25f;
    public Actor[] actorSpawns;
    public List<Actor> currentActors = new List<Actor>();
    public Actor corpseActor;
    public Image pieCirclePrefab;
    private List<Image> pieCircles = new List<Image>();
    private void Awake()
    {
        i = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            //SpawnActor(actorSpawns[Random.Range(0, actorSpawns.Length)]);
            SpawnRandomActor();

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
            bool isTicked = false;
            foreach (BehaviourComponent behaviour in actor.behaviours)
            {
                if (isTicked)
                {
                    behaviour.ticks = 0;
                }
                else if (behaviour.OnTick())
                {
                    isTicked = true;
                    behaviour.ticks = 0;
                    actor.currentBehaviour = behaviour.Name;
                }
                
            }
        }
    }
    public void SpawnActor(Actor actorToSpawn, HexCell spawnPos)
    {
        Actor spawned = Instantiate(actorToSpawn, spawnPos.transform.position, Quaternion.identity);
        currentActors.Add(spawned);
        spawned.currentTile = spawnPos;
        spawned.currentTile.JoinCell(spawned);

        spawned.deathEvent += DeathEvent;
    }

    private void SetPieChart()
    {
        
        
    }

    public void SpawnRandomActor()
    {
        var selectedTile = HexGrid.i.currentlySelected;
        var spawnByTile = actorSpawns.Where(a => (a.walkable.Contains(selectedTile.cellType))).ToList().RandomElement();
        if (spawnByTile == null) return;
        SpawnActor(spawnByTile, HexGrid.i.currentlySelected);
    }
    public void DeathEvent(StateMachine sm)
    {
        currentActors.Remove(sm as Actor);
        sm.deathEvent -= DeathEvent;
    }
    public void SpawnCorpse(HexCell tile)
    {
        SpawnActor(corpseActor, tile);
    }
}