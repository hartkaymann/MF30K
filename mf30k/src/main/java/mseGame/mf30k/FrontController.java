package mseGame.mf30k;

import player.Player;

import cards.*;

import java.util.HashMap;
import java.util.Map;
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
	private CardCreator crd_creator;
	
	@Autowired 
	private PlayerManager player_mgr;
	
	private GameStage stage;
	
	
	@PostMapping(value = "/player", consumes = "application/json")
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
		if (type.equals("door")) {
			System.out.println("DoorCard requested.");
			return crd_creator.createDoorCard();
			
		} else if (type.equals("treasure")) {
			System.out.println("TreasureCard requested");
			Card card = crd_creator.createTreasure();
			System.out.println(card);
			return card;
			
		} else {
			System.out.println("Could not determine request type.");
			return null;
		}
		//Treasure dummy = new Equipment("test", 1, 2, equipmentType.ARMOR, UUID.randomUUID());
	}
	
	@GetMapping(value = "/player/{playerID}")
	public Player getPlayer(@PathVariable String name) {
		
		return player_mgr.getPlayer(name);
		
		/*
		 * HashMap<Integer, Equipment> equip = new HashMap<Integer, Equipment>();
		HashMap<Integer, Card> hand = new HashMap<Integer, Card>();
		HashMap<Integer, Card> backPack = new HashMap<Integer, Card>();
		Equipment helmet = new Equipment("testHelmet", 1, 2, equipmentType.HELMET, 123);
		equip.put(helmet.getId(), helmet);
		Player dummy = new Player("DummyPlayer", equip, Profession.BARBARIAN, Race.DWARF, Gender.FEMALE, hand, backPack, 2, 4 );
		return dummy;
		*/
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
	
	@GetMapping(value = "/stage")
	public GameStage getStage() {
		return this.stage;
	}
	
	@PutMapping(value = "/player/{playerID}/backpack",
			consumes = "application/json")
	public void updatePlayerBackpack(@PathVariable String playerID,
			@RequestBody HashMap<UUID, Card> newCards) {
		
		Player currentPlayer = this.player_mgr.getPlayer(playerID);
		/*HashMap<UUID, Card> currentBackpack = currentPlayer.getBackpack();
		for(Map.Entry<UUID, Card> entry : new_cards.entrySet()) {
			if(!currentBackpack.containsKey(entry.getKey())) {
				//add Card
			}
		}*/
		currentPlayer.setBackpack(newCards);
		this.player_mgr.updatePlayer(playerID, currentPlayer);
		
	}
	
	@PutMapping(value = "/player/{playerID}/equipment",
			consumes = "application/json")
	public void updatePlayerEquipment(@PathVariable String playerID,
			@RequestBody HashMap<UUID, Equipment> newEquip) {
		
		Player currentPlayer = this.player_mgr.getPlayer(playerID);
		currentPlayer.setAllEquipment(newEquip);
		this.player_mgr.updatePlayer(playerID, currentPlayer);
		
	}
	

}
