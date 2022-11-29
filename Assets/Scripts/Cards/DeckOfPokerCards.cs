using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckOfPokerCards : MonoBehaviour
{
    //this class creates a deck of Poker Cards, shuffles the deck and returns it
    

    //create variables for the deck, the cardPrefab, and all of the card faces
    protected List<PokerCard> pokerDeck = new List<PokerCard>();
    [SerializeField] public GameObject cardPrefab;
    public Sprite[] cardFaces;

    int i = 0;
    
    //returns the deck
    public List<PokerCard> GetDeck
    {
        get { return pokerDeck; }
    }

    //instantiates a new poker deck
    void Start()
    {
        pokerDeck = new List<PokerCard>();        
    }    
    

    //creates a deck of poker cards for each suit and value in PokerCard.cs
    //return the created deck
    public List<PokerCard> CreateDeck()
    {
        List<PokerCard> deckOfPokerCards = new List<PokerCard>();
        foreach (PokerCard.Suit cardSuit in Enum.GetValues(typeof(PokerCard.Suit)))
        {
            foreach (PokerCard.Value cardValue in Enum.GetValues(typeof(PokerCard.Value)))
            {                
               deckOfPokerCards.Add(new PokerCard { CardSuit = cardSuit, CardValue = cardValue });
                i++;
            }
        }
        return deckOfPokerCards;        

    }  
    //shuffles the deck instantiated in Start() and returns it
    public List<PokerCard> PlayDeck()
    {
        pokerDeck = CreateDeck();
        Shuffle(pokerDeck);

        return pokerDeck;
    }
   
    //shuffles the deck using system.random
    private void Shuffle<T>(List<T> list)
    {
        
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }        

    }
}
