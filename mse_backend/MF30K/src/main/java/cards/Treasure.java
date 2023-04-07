package cards;

public class Treasure extends Card {
	protected int goldValue;
	protected int combatBonus;
	public int getGoldValue() {
		return goldValue;
	}
	public void setGoldValue(int goldValue) {
		this.goldValue = goldValue;
	}
	public int getCombatBonus() {
		return combatBonus;
	}
	public void setCombatBonus(int combatBonus) {
		this.combatBonus = combatBonus;
	}
	
}
