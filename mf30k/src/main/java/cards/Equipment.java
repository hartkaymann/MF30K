package cards;

public class Equipment extends Treasure {
	private String type;
	
	public Equipment(String _name, int _gold, int _combatBonus, String _type) {
		this.name = _name;
		this.goldValue = _gold;
		this.combatBonus = _combatBonus;
		this.type = _type;
	}
	
	public String getType() {
		return this.type;
	}
	public void setType(String _type) {
		this.type = _type;
	}
}