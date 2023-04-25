package cards;

import org.springframework.stereotype.Component;
import java.util.UUID;

@Component
public abstract class Card {
	protected String name;
	protected UUID id;
	
	public UUID getId() {
		return id;
	}
	public void setId(UUID id) {
		this.id = id;
	}
	public String getName() {
		return this.name;
	}
	public void setName(String newName) {
		this.name = newName;
	}
	
}