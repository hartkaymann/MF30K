package cards;

import java.util.UUID;
import org.springframework.stereotype.Component;

@Component
public class Monster extends Card {

	private int combatLevel;
	private int treasureAmount;
	
	public Monster(UUID _id, String _name, int combatLevel, int treasureAmount) {
		super(_name, _id, "Monster");
		this.combatLevel = combatLevel;
		this.treasureAmount = treasureAmount;
	}
	
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
