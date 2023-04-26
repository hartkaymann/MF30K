package cards;

import org.springframework.stereotype.Component;
import java.util.UUID;

@Component
public class Consumable extends Treasure {
	private boolean heroSide;
	private boolean monsterSide;
	
	public Consumable() {
		
	}
	
	public Consumable(UUID _id, String _name, int _gold, int _combat, boolean _heroSide, boolean _monsterSide) {
		super();
		this.id = _id;
		this.name = _name;
		this.goldValue = _gold;
		this.combatBonus = _combat;
		this.heroSide = _heroSide;
		this.monsterSide = _monsterSide;
	}
	
	
	public boolean isHeroSide() {
		return heroSide;
	}
	public void setHeroSide(boolean heroSide) {
		this.heroSide = heroSide;
	}
	public boolean isMonsterSide() {
		return monsterSide;
	}
	public void setMonsterSide(boolean monsterSide) {
		this.monsterSide = monsterSide;
	}
}
