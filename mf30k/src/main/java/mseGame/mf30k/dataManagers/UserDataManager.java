package mseGame.mf30k.dataManagers;

import mseGame.mf30k.Exceptions.*;
import mseGame.mf30k.repo.*;
import java.sql.Date;
import java.util.Optional;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class UserDataManager {
	
	@Autowired
	UserDataRepositoryJpa userRepo;
	
	@Autowired
	RunDataRepositoryJpa runRepo;
	
	public UserData insertUser(String username, Date registrationDate) {
		UserData newUser = userRepo.save(new UserData(username, registrationDate));
		return newUser;
	}
	
	public UserData findOne(Long id) {
		return userRepo.findById(id).get();
	}
	
	public UserData findByUserName(String username) {
		Optional<UserData> user = userRepo.findFirstByUsername(username);
//		return user.orElseThrow(() ->
//				new UserNotFoundException(username) );
		return user.orElse(null);
	}
	
	public List<UserData> findAll() {
		return userRepo.findAll();
	}
	
	public int update(UserData c) {
		UserData tmp = findOne(c.getId());

		if (tmp != null) {
			userRepo.save(c);
			return 1;
		} else {
			System.out.println("Could not update User");
			return 0;
		}
	}
	
	public int delete(UserData c) {
		UserData tmp = findOne(c.getId());
		if (tmp != null) {
			userRepo.delete(tmp);
			return 1;
		} else
			return 0;
	}
	
	@Transactional
	public UserData addRunToUser(String username, RunData r) {
		UserData user = findByUserName(username);
		user.addRun(r);
		r.setUser_owner(user);
		return user;
	}
	
	@Transactional
	public UserData removeRunFromUser(String username, RunData r) {
		UserData user = findByUserName(username);
		user.removeRun(r);
		return user;
	}
}
