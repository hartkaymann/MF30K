package mseGame.mf30k.repo;

import jakarta.persistence.*;
import jakarta.persistence.TemporalType;

import java.sql.Date;
import java.util.List;

import com.fasterxml.jackson.annotation.JsonManagedReference;

import java.util.ArrayList;

@Table(name = "users")
@Entity
public class UserData {
	
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name="user_id")
	private Long id;
	
	@Column(unique=true)
	private String username;
	
	private int wins;
	
	private int losses;
	
	@Temporal(TemporalType.DATE)
	private Date registrationDate;
	
	@OneToMany (cascade=CascadeType.ALL, fetch=FetchType.EAGER)
	@JoinColumn(name="user_owner")
	@OrderBy("id DESC")
	@JsonManagedReference
	private List<RunData> runs = new ArrayList<RunData>();

	public UserData(Long id, String username, int wins, int losses, Date registrationDate) {
		this(username, registrationDate);
		this.id = id;
	}

	public UserData(String username, Date registrationDate) {
		super();
		this.username = username;
		this.wins = 0;
		this.losses = 0;
		this.registrationDate = registrationDate;
	}

	public UserData() {
		
	}
	

	
	public List<RunData> getRuns() {
		return runs;
	}

	public void setRuns(List<RunData> runs) {
		this.runs = runs;
	}
	
	public void addRun(RunData run) {
		this.runs.add(run);
	}
	
	public void removeRun(RunData run) {
		runs.remove(run);
	}

	public Long getId() {
		return id;
	}
	
	public void setID(Long id) {
		this.id = id;
	}

	public String getUsername() {
		return username;
	}

	public void setUsername(String username) {
		this.username = username;
	}

	public int getWins() {
		return wins;
	}

	public void setWins(int wins) {
		this.wins = wins;
	}

	public int getLosses() {
		return losses;
	}

	public void setLosses(int losses) {
		this.losses = losses;
	}

	public Date getRegistrationDate() {
		return registrationDate;
	}

	public void setRegistrationDate(Date registrationDate) {
		this.registrationDate = registrationDate;
	}

}
