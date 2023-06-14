package factories;
import java.util.Random;
import java.util.UUID;

import org.springframework.stereotype.Service;

import cards.Card;
import cards.Monster;
import cards.Profession;
import cards.ProfessionCard;
import cards.Race;
import cards.RaceCard;

@Service
public class DoorCardFactory extends CardFactory {
	
	public DoorCardFactory() {
		super();
	}

	int gameProgression;
	

	public int getGameProgression() {
		return gameProgression;
	}

	public void setGameProgression(int gameProgression) {
		this.gameProgression = gameProgression;
	}

	// Create Monster
	public Monster createMonster(int gameProgression) {
		UUID id = UUID.randomUUID();
		String _name = RandomNames.randomMonsterName();
		
		//gameProgression is the changes of Stages so far. Per turn there should be around 3-4 changes
		//monster level (for single player) should then be derived from this
		//TODO for future use: Make it dependent on the player level
		int _combatLevel = rand.nextInt(gameProgression/3 + 5);
		int _treasureAmount = 1;
		if (_combatLevel <=0) {
			_combatLevel = 1;
		}
		if (_combatLevel > 5) {
			_treasureAmount = 2;
		} else if (_combatLevel > 10) {
			_treasureAmount = 3;
		}
		
		Monster monster = new Monster(id, _name, _combatLevel, _treasureAmount);
		return monster;
	}
	//Create ProfessionCard
	public ProfessionCard createProfession() {
		Profession professions[] = Profession.values();
		int index = rand.nextInt(professions.length);
		
		Profession newProf = professions[index];
		UUID newID = UUID.randomUUID();
		ProfessionCard prof = new ProfessionCard(newProf.toString(), newID, newProf);
		return prof;
	}
	
	//Create RaceCard
	public RaceCard createRace() {
		Race races[] = Race.values();
		int index = rand.nextInt(races.length);
		
		Race newRace = races[index];
		UUID newID = UUID.randomUUID();
		
		RaceCard race = new RaceCard(newRace.toString(), newID, newRace);
		return race;
	}

	@Override
	public Card createCard() {
		double chance = rand.nextDouble();
		if (chance < 0.8) {
			return createMonster(gameProgression);
		} else if(chance >= 0.8 && chance < 0.9) {
			return createProfession();
		} else if (chance >= 0.9 && chance < 1) {
			return createRace();
		} else {
			return createMonster(gameProgression);
		}
	}

}
