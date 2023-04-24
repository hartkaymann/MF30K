package cards;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import monster.Monster;

@Component
public class Spawn extends Card {
	
	//@Autowired
	public Monster spawnMonster() {
		return new Monster(
				"TestMonster", 2, 1, "stereotype");
	}
}
