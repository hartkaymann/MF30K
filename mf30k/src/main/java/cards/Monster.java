package cards;

import org.springframework.beans.factory.annotation.Autowired;
import java.util.UUID;
import org.springframework.stereotype.Component;

@Component
public class Monster extends Card {
	
	
	public Monster(UUID id, String name, int combatLevel, int treasureAmount) {
		this.id = id;
		this.name = name;
		this.combatLevel = combatLevel;
		this.treasureAmount = treasureAmount;
		//this.type = type;
	}
	
	private String name;
	private int combatLevel;
	private int treasureAmount;
	//private String type;
	
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public int getCombatLevel() {
		return combatLevel;
	}
	public void setCombatLevel(int combatLevel) {
		this.combatLevel = combatLevel;
	}
	public int getTreasureAmount() {
		return treasureAmount;
	}
	public void setTreasureAmount(int treasureAmount) {
		this.treasureAmount = treasureAmount;
	}
	/*
	public String getType() {
		return type;
	}
	public void setType(String type) {
		this.type = type;
	}*/
	
	//Losing consequences should be done in the front end, "randomly" generated there
	// maybe they could grouped into mild, hard, severe or something based on the combatLevel of the monster


}
