using System.Collections.Generic;

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public List<Run> Runs { get; set; }

    public User() { }
}
