package cards;

import java.util.Random;

public class RandomNames {

	private Random rand = new Random();
	
	private static String[] adjectives = {
			"Normal",
			"Phenomenal",
			"Cutting",
			"Fashionable",
			"Rusty",
			"Stabby",
			"Twohanded",
			"Gilded"
	};
	
	private static String[] weapons = {
		"Blade",
		"Sword",
		"Rapier",
		"Axe",
		"Dagger",
		"Halbard",
		"Stick"
	};
	
	private static String[] consumable = {
			"Grenade",
			"Stickers",
			"Konfetti",
			"Potion",
			"Brew"
	};
	
	private static String[] monsterAdjectives = {
			"Sticky",
			"Slimy",
			"Horrendous",
			"Silly",
			"Tiny",
			"Giant",
			"Bloody",
			"Colourful"
	};
	
	private static String[] monster = {
			"Slime",
			"Oger",
			"Demon",
			"Zombie"
	};
	
	public String randomConsumable() {
		return consumable[rand.nextInt(consumable.length)];
	}
	
	public String randomWeapon() {
		return weapons[rand.nextInt(weapons.length)];
	}
	
	public String randomAdjective() {
		return adjectives[rand.nextInt(adjectives.length)];
	}
	
	public String randomMonsterName() {
		String name = monsterAdjectives[rand.nextInt(monsterAdjectives.length)];
		name += " ";
		name += monster[rand.nextInt(monster.length)];
		return name;
	}
}
