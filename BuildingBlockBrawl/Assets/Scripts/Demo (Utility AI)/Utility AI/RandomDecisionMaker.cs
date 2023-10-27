using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDecisionMaker : DecisionMaker
{
    public override AIAction DecideAction(AIAction[] actions)
    {
        return actions[Random.RandomRange(0, actions.Length)];
    }
}
