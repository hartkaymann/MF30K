package mseGame.mf30k.dataManagers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import mseGame.mf30k.repo.*;

@Service
public class RunDataManager {
	@Autowired
	RunDataRepositoryJpa repo;
	
	@Autowired
	CombatDataRepository comb_repo;

	public RunData insertRun(RunData r) {
		return repo.save(r);
	}
	
	public RunData findOne(Long id) {
		return repo.findById(id).get();
	}
	
	public List<RunData> findAll() {
		return repo.findAll();
	}
	
	public int update(RunData r) {
		RunData tmp = findOne(r.getId());

		if (tmp != null) {
			repo.save(r);
			return 1;
		} else {
			System.out.println("Could not update run");
			return 0;
		}
	}
	
	public int delete(RunData c) {
		RunData tmp = findOne(c.getId());
		if (tmp != null) {
			repo.delete(tmp);
			return 1;
		} else
			return 0;
	}
	
	@Transactional
	public RunData addCombatToRun(Long run_id, CombatData combat) {
		RunData run = findOne(run_id);
		comb_repo.save(combat);
		run.addCombat(combat);
		combat.setOwner_run(run);
		return run;
	}
	
	@Transactional
	public RunData removeCombatFromRun(Long run_id, CombatData combat) {
		RunData run = findOne(run_id);
		run.removeCombat(combat);
		return run;
	}
}
