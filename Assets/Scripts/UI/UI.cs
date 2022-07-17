using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI i;
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
}
