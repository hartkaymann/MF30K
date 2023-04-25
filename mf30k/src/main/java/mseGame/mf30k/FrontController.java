package mseGame.mf30k;

import player.Player;

import cards.*;

import java.util.UUID;

import org.springframework.beans.factory.annotation.Autowired;
//import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class FrontController {
	
	//private HashMap<String, Player> players = new HashMap<String, Player>();
	@Autowired
	private GameContentManager content_mgr;
	
	@Autowired 
	private PlayerManager player_mgr;
	
	private GameStage stage;
	
	
	@PostMapping(value = "/addPlayer", consumes = "application/json")
	public void addPlayer(@RequestBody Player p) {
		System.out.println("Received Player: " + p.getName());
		player_mgr.addPlayer(p);
		return;
	}
	
	
	// Draw a Card from Treasures or Door Stack:
	// Return a random card from either Equipment or Consumable.
	// Return as JSON String
	@GetMapping(value = "/card", produces = "application/json")
	public Card drawCard(@RequestParam(name="type") String type) {
		//TODO
		//Let ContentManager create a card
		Treasure dummy = new Equipment("test", 1, 2, equipmentType.ARMOR, UUID.randomUUID());
		return dummy;
	}
	
	@GetMapping(value = "/player")
	public Player getPlayer(@RequestParam(name="name", required=true, defaultValue="Kay") String name, Model model) {
		model.addAttribute("name", name);
		
		return player_mgr.getPlayer(name);
		
		/*HashMap<Integer, Equipment> equip = new HashMap<Integer, Equipment>();
		HashMap<Integer, Card> hand = new HashMap<Integer, Card>();
		HashMap<Integer, Card> backPack = new HashMap<Integer, Card>();
		Equipment helmet = new Equipment("testHelmet", 1, 2, equipmentType.HELMET, 123);
		equip.put(helmet.getId(), helmet);
		Player dummy = new Player("DummyPlayer", equip, Profession.BARBARIAN, Race.DWARF, Gender.FEMALE, hand, backPack, 2, 4 );
		return dummy;*/
	}
	
	@PutMapping(value = "/player/{playerID}",
		consumes =  "application/json"
			)
	public void updatePlayer(@PathVariable String playerID,
			@RequestBody Player playerDetails) {
		Player updated = player_mgr.updatePlayer(playerID, playerDetails);
		System.out.println(updated);
		return;
	}
	
	@PutMapping(value = "/stage",
			consumes = "application/json")
	public void updateStage(@RequestBody GameStage stage) {
		this.stage = stage;
	}
	
	@GetMapping(value = "/getStage")
	public GameStage getStage() {
		return this.stage;
	}
	
	
	

}
