package mseGame.mf30k;

import java.util.HashMap;

import org.springframework.stereotype.Service;

import cards.Card;
import player.*;

@Service
public class PlayerManager {

	private HashMap<String, Player> players;
	
	public boolean addPlayer(Player player) {
		if(players.containsKey(player.getName())) {
			return false;
		} else {
			players.put(player.getName(), player);
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
	
	//TODO: Manage Backpack, Hand and Equipment
	
	public void updateBackpack(Card[] cards) {
		//TODO
	}
	
	public void removeCardFromBackpack(Player player, Card card) {
		//TODO
	}
	
	public void addCardToBackPack(Player player, Card card) {
		//TODO
	}
}
