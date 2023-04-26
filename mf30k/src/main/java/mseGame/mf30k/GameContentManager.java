package mseGame.mf30k;

import org.springframework.stereotype.Service;
import cards.*;
import java.util.Random;
import java.util.UUID;

@Service
public class GameContentManager {
	
	private Random rand = new Random();
	private RandomNames randNames = new RandomNames();
	
	//Create Random Treasure
	public Treasure createTreasure() {
		if(rand.nextBoolean()) {
			return createEquipment();
		}else {
			return createConsumable();
		}
	}
	
	// Create Random Equipment
	public Equipment createEquipment() {
		
		UUID _id = UUID.randomUUID();
		String _name = randNames.randomAdjective();
		int _gold = rand.nextInt(11);
		int _combat = rand.nextInt(6);
		
		equipmentType _type = null;
		
		int equip = rand.nextInt(4);
		switch(equip) {
		case 0:
			_type = equipmentType.HELMET;	
			_name += " Helmet";
			break;
		case 1:
			_type = equipmentType.ARMOR;
			_name += " Armor";
			break;
		case 2:
			_type = equipmentType.BOOTS;
			_name += " Boots";
			break;
		case 3:
			_type = equipmentType.WEAPON;
			_name += randNames.randomWeapon();
			break;
		}
		
		return new Equipment(_name, _gold, _combat, _type, _id);
	}
	
	//create Random Consumable
	public Consumable createConsumable(){
		UUID _id = UUID.randomUUID();
		String _name = randNames.randomAdjective();
		_name += " " + randNames.randomConsumable();
		int _gold = rand.nextInt(11);
		int _combat = rand.nextInt(6);
		boolean _heroside = rand.nextBoolean();
		boolean _monsterside = rand.nextBoolean();
		
		
		return new Consumable(_id, _name, _gold, _combat, _heroside, _monsterside);
	}
	
	// Create Random DoorCard
	//TODO: Profession and Race
	
	// Create Monster
	public Monster createMonster() {
		UUID id = UUID.randomUUID();
		String _name = randNames.randomMonsterName();
		int _combatLevel = rand.nextInt(25);
		int _treasureAmount = rand.nextInt(2) + 1;
		
		
		return new Monster(id, _name, _combatLevel, _treasureAmount);
	}
	
	// Track and Update Stage
	//TODO
}
