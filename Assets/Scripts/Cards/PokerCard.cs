using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerCard
{ 
    //this class contains the enums for each suit and card vale
    //an array of PokerCard is created inside the DeckOfPokerCards.cs

    public enum Suit
    {
        CLUBS, DIAMONDS,HEARTS, SPADES
    }
    public enum Value
    {
        TWO=2,THREE=3,FOUR=4,FIVE=5,SIX=6,SEVEN=7,EIGHT=8,NINE=9,TEN=10,JACK=11,QUEEN=12,KING=13,ACE=14
    }   

    //getters and setters for the suit and value
    public Suit CardSuit { get;set; }
    public Value CardValue { get;set; }    
  
}
