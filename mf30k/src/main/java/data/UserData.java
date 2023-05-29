package data;

import jakarta.persistence.*;
import jakarta.persistence.TemporalType;
import java.util.Date;
import java.util.List;

@Table(name = "users")
@Entity
public class UserData {
	
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name="user_id")
	private Long id;
	
	private String username;
	
	private int wins;
	
	private int losses;
	
	@Temporal(TemporalType.DATE)
	private Date registrationDate;
	
	@OneToMany (cascade=CascadeType.ALL, fetch=FetchType.EAGER)
	@JoinColumn(name="user_owner")
	private List<RunData> runs;

	public UserData(Long id, String username, int wins, int losses, Date registrationDate) {
		this(username, wins, losses, registrationDate);
		this.id = id;
	}

	public UserData(String username, int wins, int losses, Date registrationDate) {
		super();
		this.username = username;
		this.wins = wins;
		this.losses = losses;
		this.registrationDate = registrationDate;
	}

	public UserData() {
		
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
