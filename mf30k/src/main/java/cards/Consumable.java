package cards;

import org.springframework.stereotype.Component;

@Component
public class Consumable extends Treasure {
	private boolean heroSide;
	private boolean monsterSide;
	
	
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
