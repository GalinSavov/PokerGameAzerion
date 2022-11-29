using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class DealCards : DeckOfPokerCards
{    
    //this is the class which deals the cards to each hand(player) in a sorted order and then compares them to determine the winner
    
    //hands for all players
    private List<PokerCard> handOne;
    private List<PokerCard> handTwo; 
    private List<PokerCard> handThree;
    private List<PokerCard> handFour;

    //lists that will hold the sorted hands for each player
    private List<PokerCard> sortedHandOne;
    private List<PokerCard> sortedHandTwo;
    private List<PokerCard> sortedHandThree;
    private List<PokerCard> sortedHandFour;

    //card slots transforms and booleans, used when reordering the cards after evaluation
    [SerializeField] private Transform[] cardSlots;
    [SerializeField] private bool[] availableSlots;
    [SerializeField] private bool[] availableTexts;
    [SerializeField] private TextMeshProUGUI[] text;

    
    //puts all the instantiated game objects for each hand into its corresponding list
    private List<GameObject> handOneGameObjects;
    private List<GameObject> handTwoGameObjects;
    private List<GameObject> handThreeGameObjects;
    private List<GameObject> handFourGameObjects;


    //holds all the points for each hand
    private List<int> pointsTracker;

    void Start()
    {        
        
        handOne = new List<PokerCard>();
        handTwo = new List<PokerCard>();
        handThree = new List<PokerCard>();
        handFour = new List<PokerCard>();

        sortedHandOne = new List<PokerCard>();
        sortedHandTwo = new List<PokerCard>(); 
        sortedHandThree = new List<PokerCard>();
        sortedHandFour = new List<PokerCard>();

        
        handOneGameObjects = new List<GameObject>();
        handTwoGameObjects = new List<GameObject>();
        handThreeGameObjects = new List<GameObject>();
        handFourGameObjects = new List<GameObject>();

        pointsTracker = new List<int>();
    }
    public void Deal()
    {
        if (GetDeck.Count == 0)
        {
            PlayDeck();
            GetHand();
            SortCards();            
        }
        
    }
    //deal 5 cards for each player(hand)
    private void GetHand()
    {       
        for (int i = 0; i < 5; i++)
        {            
            handOne.Add(GetDeck[i]);           
        }      
        for (int i = 5; i < 10; i++)
        {           
            handTwo.Add(GetDeck[i]);
        }        
        for (int i = 10; i < 15; i++)
        {            
            handThree.Add(GetDeck[i]);
        }
        for (int i = 15; i < 20; i++)
        {
            handFour.Add(GetDeck[i]);
        }
    }
    //sort each hand in an ascending order
    //for each card in the sorted hand, instantiate a game object card, put it in the correct cardSlot, set its name equal to card.CardValue + card.CardSuit(this is used also in the UpdateCard.cs)
    //add each game object card to its corresponding list
    private void SortCards()
    {
        var queryHandOne = from hand in handOne
                          orderby hand.CardValue
                          select hand;
        var queryHandTwo = from hand in handTwo
                           orderby hand.CardValue
                           select hand;
        var queryHandThree = from hand in handThree
                           orderby hand.CardValue
                           select hand;
        var queryHandFour = from hand in handFour
                             orderby hand.CardValue
                             select hand;

        int i = 0;
        foreach (var card in queryHandOne)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(cardSlots[i].position.x, cardSlots[i].position.y, cardSlots[i].position.z), Quaternion.identity);
            newCard.name = card.CardValue.ToString() + card.CardSuit.ToString();
            newCard.GetComponent<SpriteRenderer>().sortingOrder = 1;
            sortedHandOne.Add(card);            
            handOneGameObjects.Add(newCard);    
            i++;
        }

        foreach (var card in queryHandTwo)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(cardSlots[i].position.x, cardSlots[i].position.y, cardSlots[i].position.z), Quaternion.identity);
            newCard.name = card.CardValue.ToString() + card.CardSuit.ToString();
            newCard.GetComponent<SpriteRenderer>().sortingOrder = 1;            
            sortedHandTwo.Add(card);
            handTwoGameObjects.Add(newCard);
            i++;
        }
        foreach (var card in queryHandThree)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(cardSlots[i].position.x, cardSlots[i].position.y, cardSlots[i].position.z), Quaternion.identity);
            newCard.name = card.CardValue.ToString() + card.CardSuit.ToString();
            newCard.GetComponent<SpriteRenderer>().sortingOrder = 1;            
            sortedHandThree.Add(card);
            handThreeGameObjects.Add(newCard);
            i++;
        }
        
        foreach (var card in queryHandFour)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(cardSlots[i].position.x, cardSlots[i].position.y, cardSlots[i].position.z), Quaternion.identity);
            newCard.name = card.CardValue.ToString() + card.CardSuit.ToString();
            newCard.GetComponent<SpriteRenderer>().sortingOrder = 1;            
            sortedHandFour.Add(card);
            handFourGameObjects.Add(newCard);
            i++;
        }
        i = 0;
        

    }
    //calls the handEvaluator.cs
    //adds the points values of each hand to the points tracker list
    //then, points list is ordered in a descending way
    //each text is assigned a value equal to the sorted points array
    //this is used for displaying the correct hands when evaluating which hand is stronger
    public void EvaluateHands()
    {
        if(GetDeck.Count == 52)
        {
            HandEvaluator handOneEvaluator = new HandEvaluator(sortedHandOne);
            HandEvaluator handTwoEvaluator = new HandEvaluator(sortedHandTwo);
            HandEvaluator handThreeEvaluator = new HandEvaluator(sortedHandThree);
            HandEvaluator handFourEvaluator = new HandEvaluator(sortedHandFour);


            HandEvaluator.Hand handOne = handOneEvaluator.EvaluateHand();
            HandEvaluator.Hand handTwo = handTwoEvaluator.EvaluateHand();
            HandEvaluator.Hand handThree = handThreeEvaluator.EvaluateHand();
            HandEvaluator.Hand handFour = handFourEvaluator.EvaluateHand();

            pointsTracker.Add(handOneEvaluator.handValues.Total);
            pointsTracker.Add(handTwoEvaluator.handValues.Total);
            pointsTracker.Add(handThreeEvaluator.handValues.Total);
            pointsTracker.Add(handFourEvaluator.handValues.Total);

            var list = pointsTracker.OrderByDescending(x => x);
            
            int k = 0;
            foreach (var item in list)
            {
                if (text[k].text == "")
                {
                    text[k].text = item.ToString();
                    k++;
                }

            }           

            EvaluateHand(handOneEvaluator, handOneGameObjects);
            EvaluateHand(handTwoEvaluator, handTwoGameObjects);
            EvaluateHand(handThreeEvaluator, handThreeGameObjects);
            EvaluateHand(handFourEvaluator, handFourGameObjects);
        }
       

    }
    

    //checks if the value of said hand = the value of the text ( the text is equal to the descending score filtered list)
    //if that is true , it also checks if the said text is not already assigned to a hand value
    //if that is also true, it places the cards in the correct positions
    public void EvaluateHand(HandEvaluator handEvaluator,List<GameObject> cards)
    {
        if (handEvaluator.handValues.Total.ToString() == text[0].text && availableTexts[0])
        {    
            if (availableSlots[0])
            {
                for (int i = 0; i < 5; i++)
                {
                    cards[i].transform.position = cardSlots[i].position;
                    availableSlots[i] = false;
                }
                availableTexts[0] = false;
            }
        }
        else if (handEvaluator.handValues.Total.ToString() == text[1].text && availableTexts[1])
        {
            if (availableSlots[5])
            {
                for (int i = 5; i < 10; i++)
                {
                    cards[i - 5].transform.position = cardSlots[i].position;
                    availableSlots[i] = false;
                }
                availableTexts[1] = false;
            }
        }
        else if (handEvaluator.handValues.Total.ToString() == text[2].text && availableTexts[2])
        {
            if (availableSlots[10])
            {
                for (int i = 10; i < 15; i++)
                {
                    cards[i - 10].transform.position = cardSlots[i].position;
                    availableSlots[i] = false;

                }
                availableTexts[2] = false;
            }
        }
       else  if (handEvaluator.handValues.Total.ToString() == text[3].text && availableTexts[3])
       {
            if (availableSlots[15])
            {
                for (int i = 15; i < 20; i++)
                {
                    cards[i - 15].transform.position = cardSlots[i].position;
                    availableSlots[i] = false;

                }
                availableTexts[3] = false;
            } 
       }
    }

}
