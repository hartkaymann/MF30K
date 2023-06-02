package mseGame.mf30k;

import player.Player;

import cards.*;

import java.util.Optional;
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
	
	@Autowired 
	private UserDataRepositoryJpa repo;
	
	@Autowired
	private RunDataRepositoryJpa runRepo;
	
	//Objects for the current game play
	private GameStage stage;
	private int stageChanges = 0;
	private UserData currentUser;
	private RunData currentRun;
	
	public int getStageChanges() {
		return this.stageChanges;
	}
	
	@GetMapping(value="/")
	public boolean connectionTest() {
		return true;
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
	
	@PostMapping(value = "/signup/{user_name}")
	public boolean addUser(@PathVariable(name="user_name")String userName) {
		long millis=System.currentTimeMillis();  
	    // creating a new object of the class Date  
	    java.sql.Date date = new java.sql.Date(millis);  
		UserData newUser = new UserData(userName, 0, 0, date);
		try {
			UserData result = repo.save(newUser);
			return true;
		} catch (Exception e) {
			System.out.println(e);
			return false;
		}

	}
	
	//login for user
	//return the userData if login was successful, return null if not.
	//sets the current user object to the logged in user.
	@PostMapping(value = "/signin/{user_name}")
	public UserData userLogin(@PathVariable(name="user_name")String userName) {
		Optional<UserData> result = repo.findFirstByUsernameOrderByIdDesc(userName);
		if(result.isPresent()) {
			UserData user = result.get();
			this.currentUser = user;
			System.out.println(user);
			return user;
		}
		System.out.println("No User with name: "+userName+" found");
		return null;
	}
	
	@GetMapping(value = "/stats/{user_name}")
	public UserData getUserData(@PathVariable(name="user_name")String user_name) {
		Optional<UserData> result = repo.findFirstByUsernameOrderByIdDesc(user_name);
		return result.orElse(null);
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
	
	@PostMapping(value="/player/{player_id}/combat")
	public void addCombatToRun(@PathVariable(name="player_id")String player_id, @RequestBody CombatData data) {
		this.currentRun.addCombat(data);
		runRepo.save(currentRun);
	}
	
	@PostMapping(value="/player/{player_id}/run")
	public boolean createNewRun(@PathVariable(name="player_id") String player_id) {
		Optional<UserData> result = repo.findFirstByUsernameOrderByIdDesc(player_id);
		if(result.isPresent()) {
			UserData user = result.get();
			RunData current = new RunData(user, 0, 0, 0, null, null);
			this.currentRun = runRepo.save(current);
			return true;
		} else {
			return false;
		}
		
	}
	
	@PutMapping(value="/player/{player_id}/run")
	public void saveRun(@PathVariable(name="player_id")String player_id, @RequestBody RunData data) {
		Optional<UserData> result = repo.findFirstByUsernameOrderByIdDesc(player_id);
		UserData user = null;
		if(result.isPresent()) {
			 user = result.get();
		} 
		
		currentRun.setCombatLevel(data.getCombatLevel());
		currentRun.setGoldsold(data.getGoldsold());
		currentRun.setPlayerLevel(data.getPlayerLevel());
		currentRun.setProfession(data.getProfession());
		currentRun.setRace(data.getRace());
		currentRun = runRepo.save(currentRun);
		//List<CombatData> combats = currentRun.getCombats();
		//data.setUser_owner(currentUser);
		//data.setCombats(combats);
		//user.addRun(data);
		user.addRun(currentRun);
		repo.save(user);
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
	
	@DeleteMapping(value="/discard")
	public void discardCard(@RequestParam(name="cardId")String id_string) {
		try {
			UUID id = UUID.fromString(id_string);
			crd_mgr.discard(id);
		} catch (Exception e) {
			System.out.println(e);
		}		
		return;
	}
	
	@DeleteMapping(value="/player/{player_id}/sell")
	public void sell(@RequestParam(name="cardId")String id_string, @PathVariable("player_id")String player_id) {
		try {
			UUID id = UUID.fromString(id_string);
			int gold = crd_mgr.sellCard(id);
			//TODO: add gold value to the players run in the database
		} catch (Exception e) {
			System.out.println(e);
		}	
	}
	

}
