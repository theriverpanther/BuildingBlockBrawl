using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DecisionMaking.UAI
{
    public class UtilityAI : DecisionMaker
    {
       public override AIAction DecideAction(AIAction[] actions)
        {
            //Loop through all the actions and score the actions

            //Score action by looping through the considerations of each action, scoring the considerations
            //then average the consideration scores to get the action score.

            //foreach(AIAction a in actions)
            //{
            //    float score = 0f;
            //    foreach (Consideration c in a.considerations)
            //    {
            //        score += c.Score();
            //    }

            //    score = score / a.considerations.Length;
            //    a.Score = score;
            //}

            float bestScore = 0f;
            AIAction bestAction = null;

            foreach(AIAction a in actions)
            {
                if(a.Score > bestScore)
                {
                    bestScore = a.Score;
                    bestAction = a;
                }
            }

            return bestAction;
        }
    }
}

