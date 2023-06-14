package mseGame.mf30k;

import java.util.UUID;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.DeleteMapping;
//import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import cards.Card;
import cards.Equipment;
import mseGame.mf30k.dataManagers.RunDataManager;
import mseGame.mf30k.dataManagers.UserDataManager;
import mseGame.mf30k.repo.CombatData;
import mseGame.mf30k.repo.RunData;
import mseGame.mf30k.repo.UserData;
import player.Player;

@RestController
public class FrontController {

	// private HashMap<String, Player> players = new HashMap<String, Player>();
	@Autowired
	private CardManager crd_mgr;

	@Autowired
	private PlayerManager player_mgr;

	@Autowired
	private RunDataManager run_mgr;

	@Autowired
	private UserDataManager user_mgr;

	// Objects for the current game play
	private GameStage stage;
	private int stageChanges = 0;
	private Long curRunID = null;
	
	public int getStageChanges() {
		return this.stageChanges;
	}

	@GetMapping(value = "/")
	public boolean connectionTest() {
		return true;
	}

	@PostMapping(value = "/player", consumes = "application/json")
	public void addPlayer(@RequestBody Player p) {
		System.out.println("Received Player: " + p.getName());
		System.out.println(p.getRace());
		System.out.println(p.getProfession());
		System.out.println(p.getGender());
		System.out.println(p.getLevel());
		System.out.println(p.getCombatLevel());
		player_mgr.addPlayer(p);
		return;
	}

	@RequestMapping(value = "/signup/{user_name}", method = { RequestMethod.GET, RequestMethod.POST })
	public boolean addUser(@PathVariable(name = "user_name") String userName) {
		UserData user = user_mgr.findByUserName(userName);
		if (user != null) {
			return false;
		} else {
			long millis = System.currentTimeMillis();
			// creating a new object of the class Date
			java.sql.Date date = new java.sql.Date(millis);
			UserData newUser = user_mgr.insertUser(userName, date);
			System.out.println(newUser);
			return true;
		}

	}

	// login for user
	// return the userData if login was successful, return null if not.
	// sets the current user object to the logged in user.
	@RequestMapping(value = "/signin/{user_name}", method = { RequestMethod.GET, RequestMethod.POST })
	public UserData userLogin(@PathVariable(name = "user_name") String userName) {

		UserData user = user_mgr.findByUserName(userName);
		return user;

	}

	@GetMapping(value = "/stats/{user_name}")
	public UserData getUserData(@PathVariable(name = "user_name") String user_name) {
		return user_mgr.findByUserName(user_name);

	}

	// Draw a Card from Treasures or Door Stack:
	// Return a random card from either Equipment or Consumable.
	// Return as JSON String
	@GetMapping(value = "/card", produces = "application/json")
	public Card drawCard(@RequestParam(name = "type") String type) {
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
		// Treasure dummy = new Equipment("test", 1, 2, equipmentType.ARMOR,
		// UUID.randomUUID());
	}

	@PostMapping(value = "/player/{player_id}/combat")
	public void addCombatToRun(@PathVariable(name = "player_id") String player_id, @RequestBody CombatData data) {
		System.out.printf("Combat Data: %b %d %d %d", data.isWin(), data.getcombatLvlPlayer(), data.getcombatLvlMonster(),
				data.getConsequence());
		if (curRunID == null) {
			System.out.println("No current Run in progress!");
			return;
		} else {
			RunData updated = run_mgr.addCombatToRun(curRunID, data);
			run_mgr.update(updated);
			return;
		}

		// this.currentRun.addCombat(data);
		// runRepo.save(currentRun);
	}

	@PostMapping(value = "/player/{player_id}/run")
	public boolean createNewRun(@PathVariable(name = "player_id") String player_id) {

		UserData user = user_mgr.findByUserName(player_id);
		if (user != null) {
			RunData current = new RunData(user, 0, 0, 0, null, null);
			current = run_mgr.insertRun(current);
			curRunID = current.getId();
			System.out.println("Current Run Id is: " + curRunID);
			return true;
		} else {
			return false;
		}

	}

	@PutMapping(value = "/player/{player_id}/run")
	public void saveRun(@PathVariable(name = "player_id") String player_id) {
		RunData currentRun = run_mgr.findOne(curRunID);

		Player data = player_mgr.getPlayer(player_id);
		currentRun.setCombatLevel(data.getCombatLevel());
		currentRun.setPlayerLevel(data.getLevel());
		currentRun.setProfession(data.getProfession());
		currentRun.setRace(data.getRace());

		run_mgr.update(currentRun);
		
		UserData user = user_mgr.findByUserName(player_id);	
		
		user_mgr.addRunToUser(player_id, currentRun);
		user_mgr.update(user);
		
		//TODO update the wins and losses

		this.curRunID = null;
		this.stageChanges = 0;
	}

	@GetMapping(value = "/player/{playerID}")
	public Player getPlayer(@PathVariable(name = "playerID") String name) {

		System.out.println(player_mgr.getPlayer(name));
		return player_mgr.getPlayer(name);
	}

	@PutMapping(value = "/player/{playerID}", consumes = "application/json")
	public void updatePlayer(@PathVariable(name = "playerID") String playerID, @RequestBody Player playerDetails) {
		Player updated = player_mgr.updatePlayer(playerID, playerDetails);
		System.out.println("Updated Player: " + updated.getName());
		System.out.println(updated.getRace());
		System.out.println(updated.getProfession());
		System.out.println(updated.getGender());
		System.out.println(updated.getLevel());
		System.out.println(updated.getCombatLevel());
		return;
	}

	@PutMapping(value = "/stage", consumes = "application/json")
	public void updateStage(@RequestBody GameStage stage) {
		this.stage = stage;
		stageChanges++;
	}

	@GetMapping(value = "/stage")
	public GameStage getStage() {
		return this.stage;
	}

	@PutMapping(value = "/player/{playerID}/backpack", consumes = "application/json")
	public void updatePlayerBackpack(@PathVariable(name = "playerID") String playerID, @RequestBody String[] IDStrings) {

		UUID[] newCardIDs = new UUID[IDStrings.length];
		for (int i = 0; i < IDStrings.length; i++) {
			newCardIDs[i] = UUID.fromString(IDStrings[i]);
		}

		Card[] updatedBackpack = new Card[newCardIDs.length];
		for (int i = 0; i < newCardIDs.length; i++) {
			updatedBackpack[i] = crd_mgr.getCardByID(newCardIDs[i]);
		}
		player_mgr.updateBackpack(updatedBackpack, playerID);
		return;
	}

	@PutMapping(value = "/player/{playerID}/equipment", consumes = "application/json")
	public void updatePlayerEquipment(@PathVariable(name = "playerID") String playerID, @RequestBody String[] IDStrings) {

		UUID[] equipIDs = new UUID[IDStrings.length];
		for (int i = 0; i < IDStrings.length; i++) {
			equipIDs[i] = UUID.fromString(IDStrings[i]);
		}

		Equipment[] newEquip = new Equipment[equipIDs.length];
		for (int i = 0; i < equipIDs.length; i++) {
			Card equipment = crd_mgr.getCardByID(equipIDs[i]);
			if (equipment instanceof Equipment) {
				newEquip[i] = (Equipment) equipment;
			} else {
				System.out.println("Wrong Card type for Equipment.");
			}
		}

		player_mgr.updateEquipment(newEquip, playerID);
		return;
	}

	@DeleteMapping(value = "/discard")
	public void discardCard(@RequestParam(name = "cardId") String id_string) {
		try {
			UUID id = UUID.fromString(id_string);
			crd_mgr.discard(id);
		} catch (Exception e) {
			System.out.println(e);
		}
		return;
	}

	@DeleteMapping(value = "/player/{player_id}/sell")
	public void sell(@RequestParam(name = "cardId") String id_string, @PathVariable("player_id") String player_id) {
		try {
			UUID id = UUID.fromString(id_string);
			int gold = crd_mgr.sellCard(id);
			RunData currentRun = run_mgr.findOne(curRunID);
			int currentGold = currentRun.getGoldsold();
			currentRun.setGoldsold(gold + currentGold);
			run_mgr.update(currentRun);
		} catch (Exception e) {
			System.out.println(e);
		}
	}

}
