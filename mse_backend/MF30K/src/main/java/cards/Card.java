package cards;

public abstract class Card {
	protected String name;
	
	public void drawCard() {
		//TODO
	}
	
	public String getName() {
		return this.name;
	}
	public void setName(String newName) {
		this.name = newName;
	}
	
}
