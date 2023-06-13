package factories;

import java.util.Random;
import java.util.UUID;

import org.springframework.stereotype.Service;

import cards.BuffTarget;
import cards.Card;
import cards.Consumable;
import cards.Equipment;
import cards.equipmentType;

@Service
public class TreasureFactory extends CardFactory {

	public TreasureFactory(Random _rand) {
		super(_rand);
	}

	// Create Random Equipment
	public Equipment createEquipment() {
		
		UUID _id = UUID.randomUUID();
		String _name = RandomNames.randomAdjective();
		
		int _combat = rand.nextInt(1, 6);
		int _gold = rand.nextInt(1,_combat+1);
		int index = rand.nextInt(equipmentType.values().length);
		equipmentType[] equip = equipmentType.values();
		
		equipmentType _type = equip[index];
		
		_name = _name + " " + _type.toString();
		
		
		Equipment equipment = new Equipment(_name, _gold, _combat, _type, _id);
		return equipment;
	}
	
	//create Random Consumable
	public Consumable createConsumable(){
		UUID _id = UUID.randomUUID();
		String _name = RandomNames.randomAdjective();
		_name += " " + RandomNames.randomConsumable();
		
		int _combat = rand.nextInt(1, 6);
		int _gold = rand.nextInt(1, _combat+1);
		
		BuffTarget targets[] = BuffTarget.values();
		int index = rand.nextInt(targets.length);
		
		BuffTarget _target = targets[index];
		
		Consumable consum =  new Consumable(_id, _name, _gold, _combat, _target);
		return consum;
	}
	
	
	@Override
	public Card createCard() {		
		if(rand.nextBoolean()) {
			return createEquipment();
		}else {
			return createConsumable();
		}
	}

}
