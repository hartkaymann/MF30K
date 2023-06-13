package mseGame.mf30k.repo;

import jakarta.persistence.*;

import java.util.List;

import com.fasterxml.jackson.annotation.JsonBackReference;
import com.fasterxml.jackson.annotation.JsonManagedReference;

import java.util.ArrayList;
import cards.Profession;
import cards.Race;

@Table(name = "runs")
@Entity
public class RunData {
	
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "run_id")
	private Long id;
	
	@ManyToOne(fetch=FetchType.EAGER, cascade=CascadeType.ALL)
	@JsonBackReference
	private UserData user_owner;
	
	private int combatLevel;
	private int playerLevel;
	private int goldsold;
	private Profession profession;
	private Race race;
	
	@OneToMany (cascade=CascadeType.ALL, fetch=FetchType.EAGER)
	@JoinColumn(name="owner_run")
	@OrderBy("id DESC")
	@JsonManagedReference
	private List<CombatData> combats = new ArrayList<CombatData>();
	
	
	public RunData(Long id, UserData user_owner, int combatLevel, int playerLevel, int goldsold,
			Profession profession, Race race) {
		this(user_owner, combatLevel, playerLevel, goldsold,
			profession, race);
		this.id = id;
	}
	
	public RunData(UserData user_owner, int combatLevel, int playerLevel, int goldsold,
			Profession profession, Race race) {
		super();
		this.user_owner = user_owner;
		this.combatLevel = combatLevel;
		this.playerLevel = playerLevel;
		this.goldsold = goldsold;
		this.profession = profession;
		this.race = race;
	}
	
	public RunData() {
		
	}
	
	
	
	public UserData getUser_owner() {
		return user_owner;
	}

	public void setUser_owner(UserData user_owner) {
		this.user_owner = user_owner;
	}

	public List<CombatData> getCombats() {
		return combats;
	}

	public void setCombats(List<CombatData> combats) {
		this.combats = combats;
	}
	
	public void addCombat(CombatData combat) {
		this.combats.add(combat);
	}
	
	public void removeCombat(CombatData combat) {
		this.combats.remove(combat);
	}
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public int getCombatLevel() {
		return combatLevel;
	}
	public void setCombatLevel(int combatLevel) {
		this.combatLevel = combatLevel;
	}
	public int getPlayerLevel() {
		return playerLevel;
	}
	public void setPlayerLevel(int playerLevel) {
		this.playerLevel = playerLevel;
	}
	public int getGoldsold() {
		return goldsold;
	}
	public void setGoldsold(int goldsold) {
		this.goldsold = goldsold;
	}
	public Profession getProfession() {
		return profession;
	}
	public void setProfession(Profession profession) {
		this.profession = profession;
	}
	public Race getRace() {
		return race;
	}
	public void setRace(Race race) {
		this.race = race;
	}
	

}
