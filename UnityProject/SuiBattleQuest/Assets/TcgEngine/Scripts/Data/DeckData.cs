using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TcgEngine
{
    /// <summary>
    /// Defines all fixed deck data (for user custom decks, check UserData.cs)
    /// </summary>
    
    [CreateAssetMenu(fileName = "DeckData", menuName = "TcgEngine/DeckData", order = 7)]
    public class DeckData : ScriptableObject
    {
        public string id;

        [Header("Display")]
        public string title;

        [Header("Cards")]
        public CardData hero;
        public CardData[] cards;

        public static List<DeckData> deck_list = new List<DeckData>();

        public static void Load(string folder = "")
        {
            if(deck_list.Count == 0)
            {
                DeckData fren_deck = CreateInstance<DeckData>();
                fren_deck.name = "SuiFrens Starter";
                fren_deck.id = "frens_deck";
                List<CardData> card_list = new();
                SuiFrensData[] frens = Resources.LoadAll<SuiFrensData>("Frens");
                fren_deck.hero = frens[0].ConvertToCard();
                foreach (SuiFrensData fren in frens)
                    card_list.Add(fren.ConvertToCard());

                List<CardData> frens_cards = new();
                for (int i = 1; i < 21; ++i)
                    frens_cards.Add(card_list[i]);

                fren_deck.cards = frens_cards.ToArray();
                deck_list.Add(fren_deck);
            }
        }

        public int GetQuantity()
        {
            return cards.Length;
        }

        public bool IsValid()
        {
            return cards.Length >= GameplayData.Get().deck_size;
        }

        public static DeckData Get(string id)
        {
            foreach (DeckData deck in GetAll())
            {
                if (deck.id == id)
                    return deck;
            }
            return null;
        }

        public static List<DeckData> GetAll()
        {
            return deck_list;
        }
    }
}