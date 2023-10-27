using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

namespace DecisionMaking.UAI
{
    public abstract class Consideration
    {
        [SerializeField] protected AnimationCurve response;

        public abstract float Score(Unit unit);

    
    }
}

