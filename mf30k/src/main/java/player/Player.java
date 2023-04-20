package player;

import java.util.HashMap;
import org.springframework.stereotype.Component;

import cards.*;

@Component
public class Player {
	
	private String name;
	private HashMap<Integer, Equipment> equipment;
	private Profession profession;
	private Race race;
	private Gender gender;
	private HashMap<Integer, Card> handCards;
	private HashMap<Integer, Card> backpack;
	private int playerLevel;
	private int combatLevel;
	
	
	public Player(String name, HashMap<Integer, Equipment> equipment, Profession profession, Race race, Gender gender,
			HashMap<Integer, Card> handCards, HashMap<Integer, Card> backpack, int playerLevel, int combatLevel) {
		super();
		this.name = name;
		this.equipment = equipment;
		this.profession = profession;
		this.race = race;
		this.gender = gender;
		this.handCards = handCards;
		this.backpack = backpack;
		this.playerLevel = playerLevel;
		this.combatLevel = combatLevel;
	}
	
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	
	public HashMap<Integer, Equipment> getEquipment() {
		return equipment;
	}
	public void setAllEquipment(HashMap<Integer, Equipment> equipment) {
		this.equipment = equipment;
	}
	
	//removes one equipment specified by key
	public boolean removeEquipment(int key) {
		if(this.equipment.containsKey(key)) {
			this.equipment.remove(key);
			return true;
		} else {
			return false;
		}
	}
	
	//adds one equipment. Returns false if the equipment already exists
	// returns true otherwise
	public boolean addOneEquipment(Equipment equip) {
		if(equipment.containsKey(equip.getId())) {
			return false;
		} else {
			equipment.put(equip.getId(), equip);
			return true;
		}
	}
	
	public Profession getProfession() {
		return profession;
	}
	public void setProfession(Profession profession) {
		this.profession = profession;
	}
	public Race getRace() {
		return race;
	}
	public void setRace(Race race) {
		this.race = race;
	}
	public Gender getGender() {
		return gender;
	}
	public void setGender(Gender gender) {
		this.gender = gender;
	}
	public HashMap<Integer, Card> getHandCards() {
		return handCards;
	}
	public boolean addHandCard(Card newCard) {
		if(handCards.containsKey(newCard.getId())) {
			return false;
		} else {
			handCards.put(newCard.getId(), newCard);
			return true;
		}
	}
	public boolean removeHandCard(Card toRemove) {
		if (handCards.containsKey(toRemove.getId())) {
			handCards.remove(toRemove.getId());
			return true;
		} else {
			return false;
		}
	}
	public HashMap<Integer, Card> getBackpack() {
		return backpack;
	}
	public void addCardToBackPack(Card newCard) {
		this.backpack.put(newCard.getId(),newCard);
	}
	public boolean removeCardFromBackpack(Card toRemove) {
		if(backpack.containsKey(toRemove.getId())) {
			backpack.remove(toRemove.getId());
			return true;
		} else {
			return false;
		}
	}
	public int getPlayerLevel() {
		return playerLevel;
	}
	public void setPlayerLevel(int playerLevel) {
		this.playerLevel = playerLevel;
	}
	public int getCombatLevel() {
		return combatLevel;
	}
	public void setCombatLevel(int combatLevel) {
		this.combatLevel = combatLevel;
	}
	

}
