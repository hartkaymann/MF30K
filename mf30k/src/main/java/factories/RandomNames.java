package factories;

import java.util.Random;

public class RandomNames {

	private static Random rand = new Random();
	
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
			/*
		"Blade",
		"Sword",
		"Rapier",
		"Axe",
		"Dagger",
		"Bow"*/
		"Monsterslayer",
		"Hellspawn",
		"Skullsplitter",
		"Faewrath",
		"Faithbringer"
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
			"Ghost"
	};
	
	public static String randomConsumable() {
		return consumable[rand.nextInt(consumable.length)];
	}
	
	public static String randomWeapon() {
		return weapons[rand.nextInt(weapons.length)];
	}
	
	public static String randomAdjective() {
		return adjectives[rand.nextInt(adjectives.length)];
	}
	
	public static String randomMonsterName() {
		String name = monsterAdjectives[rand.nextInt(monsterAdjectives.length)];
		name += " ";
		name += monster[rand.nextInt(monster.length)];
		return name;
	}
}
