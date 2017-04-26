using UnityEngine;
using System.Collections;

public interface IHeroControllable {

	void input_external_left_down ();
	
	void input_external_left_up ();
	
	void input_external_right_down ();
	
	void input_external_right_up ();
	
	void input_external_jump_down ();
	
	void input_external_jump_up ();
	
	void input_external_attack_down ();
	
	void input_external_attack_up();

	void setCameraToHero();

	void keyboard_input_detection();

	void ui_button_down_controller();

	void attack();

	void external_death();

	void external_damage();

}
