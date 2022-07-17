using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This will manage weather and spawning resources
public class WorldManager : MonoBehaviour
{
    public Actor[] Resources;

    private int dayTicks = 0;
    bool OnTick()
    {
        //The world is a statemachine that operates on day night cycles
        //When DayBehaviour ticks over we do stuff
        
        
        return true;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
