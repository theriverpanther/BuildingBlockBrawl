using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetingUIText : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private TextMeshProUGUI targetText;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(unit.name);

        targetText = GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(unit.enemies[unit.TargetIndex].name);

        //Updates the text to indicate whoever the unit is currently targeting
        //Debug.Log(unit.name + " " + unit.TargetIndex);
        if(unit.TargetIndex >= 0)
        {
            targetText.text = "(Player) " + unit.name + " is targeting " + unit.enemies[unit.TargetIndex].name;
        }



    }
}
