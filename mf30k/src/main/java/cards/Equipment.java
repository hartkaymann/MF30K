package cards;

public class Equipment extends Treasure {
	private equipmentType type;
	
	public Equipment(String _name, int _gold, int _combatBonus, equipmentType _type, int _id) {
		this.name = _name;
		this.goldValue = _gold;
		this.combatBonus = _combatBonus;
		this.type = _type;
		this.id = _id;
	}
	
	public equipmentType getType() {
		return this.type;
	}
	public void setType(equipmentType _type) {
		this.type = _type;
	}
}