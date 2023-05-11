package mseGame.mf30k;

import org.springframework.stereotype.Service;
import cards.*;

import java.util.HashMap;
import java.util.Random;
import java.util.UUID;

@Service
public class CardManager {
	
	private Random rand = new Random();
	private RandomNames randNames = new RandomNames();
	private HashMap<UUID, Card> cards = new HashMap<UUID, Card>();
	private HashMap<UUID, Card> discarded = new HashMap<UUID, Card>();
	
	
	public HashMap<UUID, Card> getCards() {
		return cards;
	}

	//Access cards by ID
	public Card getCardByID(UUID id) {
		return cards.get(id);
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
		if(rand.nextBoolean()) {
			return createEquipment();
		}else {
			return createConsumable();
		}
	}
	
	// Create Random Equipment
	public Equipment createEquipment() {
		
		UUID _id = UUID.randomUUID();
		String _name = randNames.randomAdjective();
		int _gold = rand.nextInt(11);
		int _combat = rand.nextInt(6);
		
		equipmentType _type = null;
		
		
		//TODO: Change this so its not hard coded, but like in RaceCard or Profession
		int equip = rand.nextInt(4);
		switch(equip) {
		case 0:
			_type = equipmentType.Helmet;	
			_name += " Helmet";
			break;
		case 1:
			_type = equipmentType.Armor;
			_name += " Armor";
			break;
		case 2:
			_type = equipmentType.Boots;
			_name += " Boots";
			break;
		case 3:
			_type = equipmentType.Weapon;
			_name +=  " " + randNames.randomWeapon();
			break;
		}
		Equipment equipment = new Equipment(_name, _gold, _combat, _type, _id);
		this.cards.put(_id, equipment);
		return equipment;
	}
	
	//create Random Consumable
	public Consumable createConsumable(){
		UUID _id = UUID.randomUUID();
		String _name = randNames.randomAdjective();
		_name += " " + randNames.randomConsumable();
		int _gold = rand.nextInt(11);
		int _combat = rand.nextInt(6);
		
		BuffTarget targets[] = BuffTarget.values();
		int index = rand.nextInt(targets.length);
		
		BuffTarget _target = targets[index];
		
		Consumable consum =  new Consumable(_id, _name, _gold, _combat, _target);
		this.cards.put(_id, consum);
		return consum;
	}
	
	// Create Random DoorCard
	public Card createDoorCard(int gameProgression)
	{
		double chance = rand.nextDouble();
		if (chance < 0.8) {
			return createMonster(gameProgression);
		} else if(chance >= 0.8 && chance < 0.9) {
			return createProfession();
		} else if (chance >= 0.9 && chance < 1) {
			return createRace();
		} else {
			return createMonster(gameProgression);
		}
	}
	//Create ProfessionCard
	public ProfessionCard createProfession() {
		Profession professions[] = Profession.values();
		int index = rand.nextInt(professions.length);
		
		Profession newProf = professions[index];
		UUID newID = UUID.randomUUID();
		ProfessionCard prof = new ProfessionCard(newProf.toString(), newID, newProf);
		this.cards.put(newID, prof);
		return prof;
	}
	
	//Create RaceCard
	public RaceCard createRace() {
		Race races[] = Race.values();
		int index = rand.nextInt(races.length);
		
		Race newRace = races[index];
		UUID newID = UUID.randomUUID();
		
		RaceCard race = new RaceCard(newRace.toString(), newID, newRace);
		this.cards.put(newID, race);
		return race;
	}
	
	// Create Monster
	public Monster createMonster(int gameProgression) {
		UUID id = UUID.randomUUID();
		String _name = randNames.randomMonsterName();
		
		//gameProgression is the changes of Stages so far. Per turn there should be around 3-4 changes
		//monster level (for single player) should then be derived from this
		//TODO for future use: Make it dependent on the player level
		int _combatLevel = rand.nextInt(gameProgression/3 + 5);
		int _treasureAmount = 1;
		if (_combatLevel <=0) {
			_combatLevel = 1;
		}
		if (_combatLevel > 5) {
			_treasureAmount = 2;
		} else if (_combatLevel > 10) {
			_treasureAmount = 3;
		}
		
		Monster monster = new Monster(id, _name, _combatLevel, _treasureAmount);
		this.cards.put(id, monster);
		return monster;
	}

}
