using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI i;
    public Text tickText;
    public Dice[] dice;
    public int diceRoll;
    private void Awake()
    {
        i = this;
    }
    public void _OnDiceRoll()
    {
        foreach (var item in dice)
        {
            item.RollDice();
        }
        diceRoll = 0;
    }
    private void Update()
    {
        int ticks = BehaviourManager.i.totalTicks;
        tickText.text = $"Day {1 + Mathf.CeilToInt(ticks / 60)}";
    }
}
