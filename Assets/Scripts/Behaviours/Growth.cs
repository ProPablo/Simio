using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : BehaviourComponent
{
    public int maxAge = int.MaxValue;
    public AnimationCurve sizeCurve;
    private float startScale, endScale;
    float ageTimer = 0;
    bool startAnims = false;
    public override void Start()
    {
        base.Start();
        transform.localScale = Vector3.one * sizeCurve.Evaluate(0);
    }
    private void Update()
    {
        if (!startAnims) return;
        ageTimer += Time.deltaTime;
        var normalizedScale = KongrooUtils.RemapRange(ageTimer, 0, BehaviourManager.i.tickDur, startScale, endScale);
        transform.localScale = Vector3.one * normalizedScale;
    }
    public override bool OnTick()
    {
        startScale = sizeCurve.Evaluate(KongrooUtils.RemapRange(actor.age, 0, maxAge, 0, 1));
        actor.age++;
        endScale = sizeCurve.Evaluate(KongrooUtils.RemapRange(actor.age, 0, maxAge, 0, 1));

        startAnims = true;
        if (actor.age >= maxAge)
        {
            startAnims = false;

            actor.currentHealth = 0;
            return true;
        }
        return false;
    }
}
