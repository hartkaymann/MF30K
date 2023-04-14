package player;

import java.util.ArrayList;

import org.springframework.stereotype.Component;

import cards.*;

@Component
public class Player {
	
	private String name;
	private ArrayList<Equipment> weapons;
	private Equipment armor;
	private Equipment helmet;
	private Profession profession;
	private Race race;
	private Boolean gender; //True is female, false is male
	private ArrayList<Card> handCards;
	private ArrayList<Card> backpack;
	private int playerLevel;
	private int combatLevel;
	
	
	
	
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public ArrayList<Equipment> getWeapons() {
		return weapons;
	}
	public void addWeapon(Equipment weapon) {
		this.weapons.add(weapon);
	}
	public boolean removeWeapon(Equipment weapon) {
		return this.weapons.remove(weapon);
	}
	public Equipment getArmor() {
		return armor;
	}
	public void setArmor(Equipment armor) {
		this.armor = armor;
	}
	public Equipment getHelmet() {
		return helmet;
	}
	public void setHelmet(Equipment helmet) {
		this.helmet = helmet;
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
	public Boolean getGender() {
		return gender;
	}
	public void setGender(Boolean gender) {
		this.gender = gender;
	}
	public ArrayList<Card> getHandCards() {
		return handCards;
	}
	public void addHandCard(Card newCard) {
		this.handCards.add(newCard);
	}
	public boolean removeHandCard(Card toRemove) {
		return this.handCards.remove(toRemove);
	}
	public ArrayList<Card> getBackpack() {
		return backpack;
	}
	public void addCardToBackPack(Card newCard) {
		this.backpack.add(newCard);
	}
	public boolean removeCardFromBackpack(Card toRemove) {
		return this.backpack.remove(toRemove);
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
