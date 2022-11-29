using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HandValue 
{
    //this struct is used in the HandEvaluator.cs and calculates the total points for each hand
    public int Total { get;set; }
    public int HighCard { get;set; }
}
