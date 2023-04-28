package cards;

import org.springframework.stereotype.Component;
import java.util.UUID;

@Component
public class Consumable extends Treasure {
	private BuffTarget target;
	
	public Consumable() {
		
	}
	
	public Consumable(UUID _id, String _name, int _gold, int _combat, BuffTarget _target) {
		super();
		this.id = _id;
		this.name = _name;
		this.goldValue = _gold;
		this.combatBonus = _combat;
		this.target = _target;
		this.type = "Consumable";
	}

	public BuffTarget getTarget() {
		return target;
	}

	public void setTarget(BuffTarget target) {
		this.target = target;
	}
	
}
