package cards;

import org.springframework.stereotype.Component;
import java.util.UUID;

@Component
public abstract class Card {
	protected String name;
	protected UUID id;
	protected String type;
	
	public Card() {
		
	}
	
	public Card(String _name, UUID _id, String _type) {
		this.name = _name;
		this.id = _id;
		this.type = _type;
	}
	
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
	public String getType() {
		return type;
	}
	public void setType(String type) {
		this.type = type;
	}
	
}