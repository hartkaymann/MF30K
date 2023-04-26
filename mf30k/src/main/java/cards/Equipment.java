package cards;
import java.util.UUID;

public class Equipment extends Treasure {
	private equipmentType equipType;
	
	public Equipment(String _name, int _gold, int _combatBonus, equipmentType _type, UUID _id) {
		this.name = _name;
		this.goldValue = _gold;
		this.combatBonus = _combatBonus;
		this.equipType = _type;
		this.id = _id;
		this.type = "Equipment";
	}
	
	public equipmentType getEquipType() {
		return this.equipType;
	}
	public void setEquipType(equipmentType _type) {
		this.equipType = _type;
	}
}