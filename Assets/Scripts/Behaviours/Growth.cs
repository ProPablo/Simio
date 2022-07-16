using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : BehaviourComponent
{
    public AnimationCurve sizeCurve;
    private float startScale, endScale;
    float ageTimer = 0;
    bool startAnims = false;
    public override void Start()
    {
        base.Start();
        transform.localScale = Vector3.one * sizeCurve.Evaluate(0);
        actor.totalHealthScaled = Mathf.FloorToInt(actor.baseHealth * sizeCurve.Evaluate(0));
        actor.currentHealth = actor.totalHealthScaled;
        actor.attackScaled = Mathf.FloorToInt(actor.baseAttack * sizeCurve.Evaluate(0));
    }
    private void Update()
    {
        if (!startAnims) return;
        ageTimer += Time.deltaTime;
        var normalizedScale = KongrooUtils.RemapRange(ageTimer, 0, 1 / BehaviourManager.i.tickDur, startScale, endScale);
        transform.localScale = Vector3.one * normalizedScale;
    }
    public override bool OnTick()
    {
        base.OnTick();
        startScale = sizeCurve.Evaluate(KongrooUtils.RemapRange(actor.currentAge, 0, actor.maxAge, 0, 1));
        actor.currentAge++;
        endScale = sizeCurve.Evaluate(KongrooUtils.RemapRange(actor.currentAge, 0, actor.maxAge, 0, 1));

        // Stat scaling
        actor.totalHealthScaled = Mathf.FloorToInt(actor.baseHealth * sizeCurve.Evaluate(actor.currentAge / actor.maxAge));
        actor.attackScaled = Mathf.FloorToInt(actor.baseAttack * sizeCurve.Evaluate(actor.currentAge / actor.maxAge));

        startAnims = true;
        if (actor.currentAge >= actor.maxAge)
        {
            startAnims = false;

            actor.currentHealth = 0;
            return true;
        }
        return false;
    }
}
