package data;

import jakarta.persistence.*;

public class CombatData {
	
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name="combat_id")
	private Long id;
	
	private int combatLvlPlayer;
	private int combatLvlMonster;
	private boolean win;
	
	private int consequence;

	@ManyToOne(fetch=FetchType.EAGER, cascade=CascadeType.ALL)
	private RunData owner_run;

	public CombatData(Long id, int combatLvlPlayer, int combatLvlMonster, boolean win, RunData owner_run) {
		this(combatLvlPlayer, combatLvlMonster, win, owner_run);
		this.id = id;		
	}	
	
	public CombatData(int combatLvlPlayer, int combatLvlMonster, boolean win, RunData owner_run) {
		super();
		this.combatLvlPlayer = combatLvlPlayer;
		this.combatLvlMonster = combatLvlMonster;
		this.win = win;
		this.owner_run = owner_run;
	}
	
	public CombatData() {
		
	}
	

	public Long getId() {
		return id;
	}

	public void setId(Long id) {
		this.id = id;
	}

	public int getcombatLvlPlayer() {
		return combatLvlPlayer;
	}

	public void setcombatLvlPlayer(int combatLvlPlayer) {
		this.combatLvlPlayer = combatLvlPlayer;
	}

	public int getcombatLvlMonster() {
		return combatLvlMonster;
	}

	public void setcombatLvlMonster(int combatLvlMonster) {
		this.combatLvlMonster = combatLvlMonster;
	}

	public boolean isWin() {
		return win;
	}

	public void setWin(boolean win) {
		this.win = win;
	}

	public RunData getOwner_run() {
		return owner_run;
	}

	public void setOwner_run(RunData owner_run) {
		this.owner_run = owner_run;
	}
	
	public int getConsequence() {
		return consequence;
	}

	public void setConsequence(int consequence) {
		this.consequence = consequence;
	}
	
	
}
