package cards;

import java.util.UUID;

public class ProfessionCard extends Card {

	private Profession profession;
	


	public ProfessionCard(String _name, UUID _id, Profession profession) {
		super(_name, _id, "Profession");
		this.profession = profession;
	}

	public Profession getProfession() {
		return profession;
	}

	public void setProfession(Profession profession) {
		this.profession = profession;
	}
	
	
	
}
