package factories;

import java.util.Random;

import cards.*;

public abstract class CardFactory {
	
	protected Random rand = new Random();
	
	public CardFactory() {

	}
	
	public Random getRand() {
		return rand;
	}

	public void setRand(Random rand) {
		this.rand = rand;
	}

	public abstract Card createCard();

}
