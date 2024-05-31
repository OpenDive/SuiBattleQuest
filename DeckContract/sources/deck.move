module deck::DeckModule {
    use std::vector;
    use std::option::Self;

    use sui::dynamic_object_field as ofield;
    use sui::dynamic_field as df;
    use sui::transfer;
    use sui::object::{Self, UID};
    use sui::tx_context::{Self, TxContext};
    use std::bcs;

    struct Deck has key, store {
        id: UID,
        name: vector<u8>,
        cards: vector<UID>,
    }

    struct DeckManager has key, store {
        id: UID,
        decks: vector<Deck>,
    }

    /// The deck was not able to be found.
    const E_DECK_NOT_FOUND: u64 = 1;

    /// The card was not able to be found.
    const E_CARD_NOT_FOUND: u64 = 2;

    /// The deck has already been created.
    const E_DECK_ALREADY_EXISTS: u64 = 3;

    public fun create_deck(name: vector<u8>, manager: &mut UID, ctx: &mut TxContext) {
        let deck = Deck { 
            id: object::new(ctx), 
            name, 
            cards: vector::empty<UID>() 
        };
        add_deck_to_deck_manager(
            ofield::borrow_mut(
                manager,
                b"deckmanager"
            ),
            deck
        );
    }

    public fun create_deck_manager(decks: vector<Deck>, ctx: &mut TxContext) {
        let uid = object::new(ctx);
        let deck_manager = DeckManager { id: &uid, decks };
        ofield::add(
            &mut uid,
            b"deckmanager",
            deck_manager
        );
    }

    public fun add_deck_to_deck_manager(manager: &mut DeckManager, deck: Deck) {
        assert!(!(is_deck_exist(&manager.decks, &deck.name)), E_DECK_ALREADY_EXISTS);
        vector::push_back(&mut manager.decks, deck);
    }

    // public entry fun add_card(deck_name: vector<u8>, card_id: UID, ctx: &mut TxContext) {
    //     let manager = borrow_global_mut<DeckManager>(tx_context::sender(ctx));
    //     let deck_option = find_deck_by_name(&mut manager.decks, &deck_name);
    //     assert!(!(option::is_none(&deck_option)), E_DECK_NOT_FOUND);
    //     let deck = option::borrow_mut(&deck_option).unwrap();
    //     vector::push_back(&mut deck.cards, card_id);
    // }

    // public entry fun remove_card(deck_name: vector<u8>, card_id: UID, ctx: &mut TxContext) {
    //     let manager = borrow_global_mut<DeckManager>(tx_context::sender(ctx));
    //     let deck_option = find_deck_by_name(&mut manager.decks, &deck_name);
    //     assert!(!(option::is_none(&deck_option)), E_DECK_NOT_FOUND);
    //     let deck = option::borrow_mut(&deck_option).unwrap();
    //     let index_option = vector::index_of(&deck.cards, &card_id);
    //     assert!(!(option::is_none(&index_option)), E_CARD_NOT_FOUND);
    //     let index = option::unwrap(index_option);
    //     vector::remove(&mut deck.cards, index);
    // }

    // fun find_deck_by_name(decks: &mut vector<Deck>, name: &vector<u8>) : option::Option<&mut Deck> {
    //     let i = 0;
    //     while (i < vector::length(decks)) {
    //         let deck = vector::borrow_mut(decks, i);
    //         if (vector::equals(&deck.name, name)) {
    //             return option::some(deck);
    //         };
    //         i = i + 1;
    //     };
    //     return option::none;
    // }

    fun is_deck_exist(decks: &vector<Deck>, name: &vector<u8>) : bool {
        let i = 0;
        let res = false;
        while (i < vector::length(decks)) {
            let deck = vector::borrow(decks, i);
            if (&deck.name == name) {
                res = true;
            };
            i = i + 1;
        };
        res
    }
}
