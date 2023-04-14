package mseGame.mf30k;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class GameController {
	
	// Draw a Card from Treasures:
	// Return a random card from either Equipment or Consumable.
	// Return as JSON String
	@GetMapping(value = "/doorcard")
	public String drawTreasure(Model model) {
		
		return "door: Race, Class or Spawn";
	}
	
	@GetMapping(value = "/player")
	public String getPlayer(@RequestParam(name="name", required=true, defaultValue="Kay") String name, Model model) {
		model.addAttribute("name", name);
		
		return "test";
	}
	

}
