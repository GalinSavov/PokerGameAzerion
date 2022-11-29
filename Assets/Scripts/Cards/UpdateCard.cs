using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCard : MonoBehaviour
{

    //this class makes sure that each instantiated card prefab has the correct card face attached to it

    //variables for the card face sprite, as well as a reference to the DeckOfPokerCards.cs for the deck
    [SerializeField] private Sprite cardFace;
    private SpriteRenderer spriteRenderer;
    private DeckOfPokerCards deckOfPokerCards;    
    
    void Start()
    {
        //get a reference to DeckOfPokerCards.cs
        //create a list which holds the values of the shuffled deck
        deckOfPokerCards = FindObjectOfType<DeckOfPokerCards>();
        List<PokerCard> deck = deckOfPokerCards.CreateDeck();


        //loop through the deck
        //if the name of the instantiated card object matches the one in the deck , assign the correct card face
        // the card face[] is modified in the inspector where I have assigned all 52 cards in ascending value
        int i = 0;
        foreach (PokerCard card in deck)
        {
            if(this.name == card.CardValue.ToString()+ card.CardSuit.ToString())           
            {
                cardFace = deckOfPokerCards.cardFaces[i];
                
                break;
            }
            i++;

        }    
        //assign the correct sprite to the card
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = cardFace;

    }

}
