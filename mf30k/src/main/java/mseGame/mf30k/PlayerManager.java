package mseGame.mf30k;

import java.util.HashMap;
import java.util.UUID;
import org.springframework.stereotype.Service;

import cards.*;
import player.*;

@Service
public class PlayerManager {

	private HashMap<String, Player> players = new HashMap<>();
	
	public boolean addPlayer(Player p) {
		System.out.println("Adding Player: " + p.getName());
		System.out.println(p.getRace());
		System.out.println(p.getProfession());
		System.out.println(p.getGender());
		System.out.println(p.getPlayerLevel());
		System.out.println(p.getCombatLevel());
		if(players.containsKey(p.getName())) {
			return false;
		} else {
			players.put(p.getName(), p);
			System.out.println(players);
			return true;
		}
	}
	
	public boolean removePlayer(Player player) {
		if(players.containsKey(player.getName())) {
			Player removed = players.remove(player.getName());
			System.out.println("Removed: " + removed.getName());
			return true;
		} else {
			System.out.println("Player does not exist and can therefore not be removed.");
			return false;
		}
	}
	
	public Player getPlayer(String name) {
		return players.get(name);
	}
	
	public Player updatePlayer(String name, Player updatedPlayer) {
		Player plr = players.get(name);
		plr.setCombatLevel(updatedPlayer.getCombatLevel());
		plr.setPlayerLevel(updatedPlayer.getPlayerLevel());
		plr.setGender(updatedPlayer.getGender());
		plr.setProfession(updatedPlayer.getProfession());
		plr.setRace(updatedPlayer.getRace());
		players.replace(name, plr);
		return players.get(name);
	}
	

	//Update Backpack and Equipment
	public void updateBackpack(Card[] cards, String playerID) {
		Player currentPlayer = players.get(playerID);
		HashMap<UUID, Card> newBackpack = new HashMap<UUID, Card>();
		for(int i = 0; i < cards.length; i++) {
			newBackpack.put(cards[i].getId(), cards[i]);
		}
		currentPlayer.setBackpack(newBackpack);
		players.replace(playerID, currentPlayer);
		return;
	}
	
	public void updateEquipment(Equipment[] equip, String playerID) {
		Player currentPlayer = players.get(playerID);
		HashMap<UUID, Equipment> newEquip = new HashMap<UUID, Equipment>();
		for(int i = 0; i < equip.length; i++) {
			newEquip.put(equip[i].getId(), equip[i]);
		}
		currentPlayer.setAllEquipment(newEquip);
		players.replace(playerID, currentPlayer);
		return;		
	}
}
