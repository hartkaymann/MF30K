package cards;

import java.util.UUID;

public class RaceCard extends Card {

	private Race race;

	public RaceCard(String _name, UUID _id, Race race) {
		super(_name, _id, "Race");
		this.race = race;
	}

	public Race getRace() {
		return race;
	}

	public void setRace(Race race) {
		this.race = race;
	}
	
	
	
	
}
