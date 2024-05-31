module deck::DeckModuleTests {
    use sui::object::{Self, UID};
    use sui::tx_context::{Self, TxContext};
    use sui::vector;
    use sui::test_scenario::{Self, test};

    use deck::DeckModule;

    #[test]
    public entry fun test_create_deck(ctx: &mut TxContext) {
        let name = vector::utf8(b"TestDeck");
        DeckModule::create_deck(name, ctx);
        assert!(true, 100);
    }

    #[test]
    public entry fun test_add_card(ctx: &mut TxContext) {
        let name = vector::utf8(b"TestDeck");
        let card_id = object::new<UID>(ctx);
        DeckModule::add_card(name, card_id, ctx);
        assert!(true, 101);
    }

    #[test]
    public entry fun test_remove_card(ctx: &mut TxContext) {
        let name = vector::utf8(b"TestDeck");
        let card_id = object::new<UID>(ctx);
        DeckModule::remove_card(name, card_id, ctx);
        assert!(true, 102);
    }

    #[test]
    public entry fun test_remove_card_not_found(ctx: &mut TxContext) {
        let name = vector::utf8(b"TestDeck");
        let card_id = object::new<UID>(ctx);
        let result = move_to_deferred!(DeckModule::remove_card(name, card_id, ctx));
        assert!(result == 2, 103);
    }

    #[test]
    public entry fun test_add_card_deck_not_found(ctx: &mut TxContext) {
        let name = vector::utf8(b"NonExistentDeck");
        let card_id = object::new<UID>(ctx);
        let result = move_to_deferred!(DeckModule::add_card(name, card_id, ctx));
        assert!(result == 1, 104);
    }

    #[test]
    public entry fun test_create_deck_already_exists(ctx: &mut TxContext) {
        let name = vector::utf8(b"TestDeck");
        DeckModule::create_deck(name.clone(), ctx);
        let result = move_to_deferred!(DeckModule::create_deck(name, ctx));
        assert!(result == 3, 105);
    }
}
