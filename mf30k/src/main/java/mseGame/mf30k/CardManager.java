package mseGame.mf30k;

import org.springframework.stereotype.Service;
import cards.*;
import factories.*;

import java.util.HashMap;
import java.util.UUID;
import java.util.Random;

@Service
//@ComponentScan(basePackages = {"factories"} )
public class CardManager {

	private HashMap<UUID, Card> cards = new HashMap<UUID, Card>();
	private HashMap<UUID, Card> discarded = new HashMap<UUID, Card>();
	
	private TreasureFactory treasures = new TreasureFactory();
	private DoorCardFactory doors = new DoorCardFactory();
	
	public HashMap<UUID, Card> getCards() {
		return cards;
	}

	//Access cards by ID
	public Card getCardByID(UUID id) {
		return cards.get(id);
	}
	
	//Sell Card (discard, but return gold value)
	public int sellCard(UUID id) {
		Card to_sell = cards.get(id);
		if(to_sell instanceof Treasure) {
			int goldValue = ((Treasure) to_sell).getGoldValue();
			this.discard(id);
			return goldValue;
		} else {
			System.out.println("Card is not a treasure and therefore cannot be sold");
			return -1;
		}
	}
	
	//Discard card by ID
	public void discard(UUID id) {
		Card to_discard = cards.get(id);
		cards.remove(id);
		discarded.put(id, to_discard);
		return;
	}
	
	//Access the discarded Cards by ID
	public Card getDiscardedByID(UUID id) {
		return discarded.get(id);
	}
	
	//Create Random Treasure
	public Treasure createTreasure() {
		Card tres = treasures.createCard();
		if(tres instanceof Treasure) {
			cards.put(tres.getId(), tres);
			return (Treasure)tres;
		} else {
			return null;
		}
		
	}
	
	// Create Random DoorCard
	public Card createDoorCard(int gameProgression)
	{
		doors.setGameProgression(gameProgression);
		Card door = doors.createCard();
		cards.put(door.getId(), door);
		return door;
	}
	

}
