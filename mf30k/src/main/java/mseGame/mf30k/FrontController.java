package mseGame.mf30k;

import player.Player;

import cards.*;

import java.util.HashMap;
import java.util.UUID;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.DeleteMapping;
//import org.springframework.stereotype.Controller;
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
	private CardManager crd_mgr;
	
	@Autowired 
	private PlayerManager player_mgr;
	
	private GameStage stage;
	private int stageChanges = 0;
	
	public int getStageChanges() {
		return this.stageChanges;
	}
	
	
	@PostMapping(value = "/player", consumes = "application/json")
	public void addPlayer(@RequestBody Player p) {
		System.out.println("Received Player: " + p.getName());
		System.out.println(p.getRace());
		System.out.println(p.getProfession());
		System.out.println(p.getGender());
		System.out.println(p.getPlayerLevel());
		System.out.println(p.getCombatLevel());
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
			Card card = crd_mgr.createDoorCard(stageChanges);
			System.out.println(crd_mgr.getCards());
			return card;
			
		} else if (type.equals("treasure")) {
			System.out.println("TreasureCard requested");
			Card card = crd_mgr.createTreasure();
			System.out.println(crd_mgr.getCards());
			System.out.println(card);
			return card;
			
		} else {
			System.out.println("Could not determine request type.");
			return null;
		}
		//Treasure dummy = new Equipment("test", 1, 2, equipmentType.ARMOR, UUID.randomUUID());
	}
	
	@GetMapping(value = "/player/{playerID}")
	public Player getPlayer(@PathVariable(name = "playerID") String name) {
		
		System.out.println(player_mgr.getPlayer(name));
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
	public void updatePlayer(@PathVariable(name = "playerID") String playerID,
			@RequestBody Player playerDetails) {
		Player updated = player_mgr.updatePlayer(playerID, playerDetails);
		System.out.println(updated);
		return;
	}
	
	@PutMapping(value = "/stage",
			consumes = "application/json")
	public void updateStage(@RequestBody GameStage stage) {
		this.stage = stage;
		stageChanges++;
	}
	
	@GetMapping(value = "/stage")
	public GameStage getStage() {
		return this.stage;
	}
	
	@PutMapping(value = "/player/{playerID}/backpack",
			consumes = "application/json")
	public void updatePlayerBackpack(@PathVariable(name = "playerID") String playerID,
			@RequestBody String[] IDStrings) {
		
		UUID[] newCardIDs = new UUID[IDStrings.length];
		for (int i = 0; i < IDStrings.length; i++) {
			newCardIDs[i] = UUID.fromString(IDStrings[i]);
		}
				
		Card[] updatedBackpack = new Card[newCardIDs.length];
		for(int i = 0; i < newCardIDs.length; i++) {
			updatedBackpack[i] = crd_mgr.getCardByID(newCardIDs[i]);
		}
		player_mgr.updateBackpack(updatedBackpack, playerID);
		return;
	}
	
	@PutMapping(value = "/player/{playerID}/equipment",
			consumes = "application/json")
	public void updatePlayerEquipment(@PathVariable(name = "playerID") String playerID,
			@RequestBody String[] IDStrings) {
		
		UUID[] equipIDs = new UUID[IDStrings.length];
		for (int i = 0; i < IDStrings.length; i++) {
			equipIDs[i] = UUID.fromString(IDStrings[i]);
		}
		
		Equipment[] newEquip = new Equipment[equipIDs.length];
		for(int i = 0; i < equipIDs.length; i++) {
			Card equipment = crd_mgr.getCardByID(equipIDs[i]);
			if(equipment instanceof Equipment) {
				newEquip[i] = (Equipment) equipment;
			} else {
				System.out.println("Wrong Card type for Equipment.");
			}
		}
		
		player_mgr.updateEquipment(newEquip, playerID);
		return;
	}
	
	@DeleteMapping(value="/card")
	public void discardCard(@RequestParam(name="cardId")String id_string) {
		try {
			UUID id = UUID.fromString(id_string);
			crd_mgr.discard(id);
		} catch (Exception e) {
			System.out.println(e);
		}		
		return;
	}
	

}
