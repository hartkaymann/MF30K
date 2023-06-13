package mseGame.mf30k.Exceptions;

import java.text.MessageFormat;

public class UserNotFoundException extends RuntimeException {
	
	private static final long serialVersionUID = 1L;

	public UserNotFoundException(String username) {
		super(MessageFormat.format("Could not find user with name {0}", username));
	}

}
