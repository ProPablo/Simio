using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public float tickDur = 0.5f;
    float timer;
    public List<Actor> actors = new List<Actor>();
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = tickDur;
            foreach (Actor actor in actors)
            {
                foreach (TickComponent behaviour in actor.behaviours)
                {
                    behaviour.OnTick();
                }
            }
        }
    }
}
