using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TcgEngine.UI
{
    /// <summary>
    /// Visual representation of a card in your collection in the Deckbuilder
    /// </summary>

    public class CollectionCard : MonoBehaviour
    {
        public CardUI card_ui;

        [Header("Mat")]
        public Material color_mat;
        public Material grayscale_mat;

        public UnityAction<CardUI> onClick;
        public UnityAction<CardUI> onClickRight;

        private void Start()
        {
            card_ui.onClick += onClick;
            card_ui.onClickRight += onClickRight;
        }

        public void SetCard(CardData card, VariantData variant)
        {
            card_ui.SetCard(card, variant);
        }

        public void SetGrayscale(bool grayscale)
        {
            card_ui.SetMaterial(color_mat);
        }

        public CardData GetCard()
        {
            return card_ui.GetCard();
        }

        public VariantData GetVariant()
        {
            return card_ui.GetVariant();
        }
    }
}