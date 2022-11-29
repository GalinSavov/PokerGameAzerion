using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEvaluator : PokerCard
{
    //this class evaluates each hand and returns its value
    //I have made a points system for each hand, so that I can differentiate between cases like : 2 hands with OnePair, 2 hands
    //with ThreeOfAKind, etc.
  
    //a hand enum with all possible hand combinations, including a high card only
    public enum Hand { HighCard = 0, OnePair=15, TwoPairs=25, ThreeOfAKind=100, Straight=1000, Flush=10000, FullHouse=100000, FourOfAKind = 1000000, StraightFlush =10000000, RoyalFlush = 100000000 }

    //the hand value struct
    private HandValue handValue;

    //those 4 variables keep track of how many of a suit a hand has
    // it is very helpful for flushes
    private int heartsSum;
    private int clubsSum;
    private int diamondsSum;
    private int spadesSum;

    private List<PokerCard> cards;
    

   //this handEvaluator is called in DealCards.cs and inside it is passed the sorted hand, filtered using LINQ
    public HandEvaluator(List<PokerCard> sortedHand)
    {
        heartsSum = 0;
        clubsSum = 0;
        diamondsSum = 0;
        spadesSum = 0;
        cards = new List<PokerCard>();
        Cards = sortedHand;
        handValue = new HandValue();        
    }

    //getter and setter for HandValue struct
    public HandValue handValues
    {

        get{ return handValue; }
        set{ handValue = value; }
    }

    //
    public List<PokerCard> Cards
    {       
        
        get { return cards; }
        set
        {            
            cards = value;            
        }
        
    }

    //evaluates the hand
    //return the according enum and points
    //the if statements are written in descending order, otherwise the check will stop at e.g. OnePair and not check for TwoPairs if the order is ascending
    //if none of the combinations are true, returns a high card
    //each hand is sorted in ascending order in DealCards.cs, that is why I check for combinations at specific indexes
    public Hand EvaluateHand()
    {
        GetNumberOfSuits();
        if (RoyalFlush())
            return Hand.RoyalFlush;
        else if (StraightFlush())
            return Hand.StraightFlush;
        else if (FourOfAKind())
            return Hand.FourOfAKind;
        else if (FullHouse())
            return Hand.FullHouse;
        else if (Flush())
            return Hand.Flush;
        else if (Straight())
            return Hand.Straight;
        else if (ThreeOfAKind())
            return Hand.ThreeOfAKind;
        else if (TwoPairs())
            return Hand.TwoPairs;
        else if (Pair())
            return Hand.OnePair;
        else if(HighCard())
            return Hand.HighCard;      
         

        
        return Hand.HighCard + handValue.HighCard;
    }
    //checks how many of a suit each hand has
    private void GetNumberOfSuits()
    {
        foreach (var card in Cards)
        {
            if (card.CardSuit == PokerCard.Suit.CLUBS)
                clubsSum++;
            else if (card.CardSuit == PokerCard.Suit.HEARTS)
                heartsSum++;
            else if (card.CardSuit == PokerCard.Suit.DIAMONDS)
                diamondsSum++;
            else if (card.CardSuit == PokerCard.Suit.SPADES)
                spadesSum++;
        }
    }
  
    //the total value of the hand is the last card[4] value (which is always the highest because of the sorting done in DealCards.cs) + the Hand.HighCard
    private bool HighCard()
    {
        handValue.Total = (int)cards[4].CardValue + (int)Hand.HighCard;
            return true;
    }
    //checks for all possible pair combinations and returns the card[i] * 2 + the Hand.OnePair
    private bool Pair()
    {
        if(cards[0].CardValue == cards[1].CardValue)
        {
            handValue.Total = (int)cards[0].CardValue * 2 + (int)Hand.OnePair;
            return true;
        }
        else if (cards[1].CardValue == cards[2].CardValue)
        {
            handValue.Total = (int)cards[1].CardValue * 2 + (int)Hand.OnePair;
            return true;
        }
        else if (cards[2].CardValue == cards[3].CardValue)
        {
            handValue.Total = (int)cards[2].CardValue * 2 + (int)Hand.OnePair;
            return true;
        }
        else if (cards[3].CardValue == cards[4].CardValue)
        {
            handValue.Total = (int)cards[3].CardValue * 2 + (int)Hand.OnePair;
            return true;
        }

        return false;
    }
    //checks for all possible two pairs combinations and returns the card[firstPair] * 2 + card[secondPair] * 2 + Hand.TwoPairs
    private bool TwoPairs()
    {
        if (cards[0].CardValue == cards[1].CardValue && cards[2].CardValue == cards[3].CardValue)
        {
            handValue.Total = ((int)cards[0].CardValue * 2) + ((int)cards[2].CardValue * 2) + (int)Hand.TwoPairs;            
            return true;
        }
        else if(cards[0].CardValue == cards[1].CardValue && cards[3].CardValue == cards[4].CardValue)
        {
            handValue.Total = ((int)cards[1].CardValue * 2) + ((int)cards[3].CardValue * 2) + (int)Hand.TwoPairs;           
            return true;
        }
        else if (cards[1].CardValue == cards[2].CardValue && cards[3].CardValue == cards[4].CardValue)
        {
            handValue.Total = ((int)cards[1].CardValue * 2) + ((int)cards[3].CardValue * 2) + (int)Hand.TwoPairs;           
            return true;
        }
        return false;
    }
    //checks for all possible three of a kind combinations and returns the card value [i] * 3 + Hand.ThreeOfAKind
    private bool ThreeOfAKind()
    {
        if (cards[0].CardValue == cards[1].CardValue && cards[0].CardValue == cards[2].CardValue)
        {
            handValue.Total = (int)cards[0].CardValue * 3 + (int)Hand.ThreeOfAKind;
            return true;
        }
        else if (cards[1].CardValue == cards[2].CardValue && cards[1].CardValue == cards[3].CardValue)
        {
            handValue.Total = (int)cards[1].CardValue * 3 + (int)Hand.ThreeOfAKind;
            return true;
        }
        else if (cards[2].CardValue == cards[3].CardValue && cards[2].CardValue == cards[4].CardValue)
        {
            handValue.Total = (int)cards[2].CardValue * 3 + (int)Hand.ThreeOfAKind;
            return true;
        }

        return false;
    }
    //checks if the straight combination exists and return the value of each card + Hand.Straight
    private bool Straight()
    {
        if (cards[0].CardValue + 1 == cards[1].CardValue &&
            cards[1].CardValue + 1 == cards[2].CardValue &&
            cards[2].CardValue + 1 == cards[3].CardValue &&
            cards[3].CardValue + 1 == cards[4].CardValue)
        {
            handValue.Total = (int)cards[0].CardValue + (int)cards[1].CardValue + (int)cards[2].CardValue + 
                (int)cards[3].CardValue + (int)cards[4].CardValue + (int)Hand.Straight;

            return true;
        }
        return false;
    }
    
    //checks if the sum of a suit is 5 for each suit and returns the hand
    private bool Flush()
    {
        if(heartsSum==5 || clubsSum==5 || diamondsSum == 5 || spadesSum == 5)
        {
            handValue.Total = (int)cards[4].CardValue * 5 + (int)Hand.Flush;
            return true;
        }
        return false;
    }
    //checks for both combinations of a full house and returns the value
    private bool FullHouse()
    {
        if (cards[0].CardValue == cards[1].CardValue && cards[0].CardValue == cards[2].CardValue && cards[3].CardValue == cards[4].CardValue)
        {
            handValue.Total = (int)cards[0].CardValue * 3  + (int)cards[3].CardValue * 2 + (int)Hand.FullHouse;
            return true;
        }
        else if (cards[0].CardValue == cards[1].CardValue && cards[2].CardValue == cards[3].CardValue && cards[2].CardValue == cards[4].CardValue)
        {
            handValue.Total = (int)cards[0].CardValue * 2 + (int)cards[2].CardValue * 3 + (int)Hand.FullHouse;
            return true;
        }
        return false;
    }
    //checks for both combinations of four of a kind and return the value
    private bool FourOfAKind()
    {
        if (cards[0].CardValue == cards[1].CardValue && cards[0].CardValue == cards[2].CardValue && cards[0].CardValue == cards[3].CardValue ||
            cards[1].CardValue == cards[2].CardValue && cards[1].CardValue == cards[3].CardValue && cards[1].CardValue == cards[4].CardValue)
        {
            handValue.Total = (int)cards[1].CardValue * 4 + (int)Hand.FourOfAKind;
            return true;

        }
        return false;
    }
    //checks if the card[0] + 1 = card[1] etc. and checks if the sum of a suit is 5
    private bool StraightFlush()
    {
        if(cards[0].CardValue + 1 == cards[1].CardValue &&cards[1].CardValue + 1 == cards[2].CardValue &&cards[2].CardValue + 1 == cards[3].CardValue &cards[3].CardValue + 1 == cards[4].CardValue)
            if(heartsSum == 5 || clubsSum == 5 || diamondsSum == 5 || spadesSum == 5)
            {
                handValue.Total = (int)cards[4].CardValue * 5 + (int)Hand.StraightFlush;
                return true;
            }       
        return false;
    }
    //checks if the hand is 10,J,Q,K,A and if the suit =5 and returns the value
    private bool RoyalFlush()
    {
        if(cards[0].CardValue == Value.TEN &&cards[1].CardValue == Value.JACK &&cards[2].CardValue == Value.QUEEN &&cards[3].CardValue == Value.KING &&cards[4].CardValue == Value.ACE)
            if (heartsSum == 5 || clubsSum == 5 || diamondsSum == 5 || spadesSum == 5)
            {
                handValue.Total = (int)cards[4].CardValue * 5 + (int)Hand.RoyalFlush;
                return true;
            }        
        return false;
    }
    
        
    
    
}
