using System.Collections.Generic;

public class Run
{

    public int Id { get; set; }
    public int Level { get; set; }
    public int CombatLevel { get; set; }
    public int GoldSold { get; set; }
    public Profession Profession { get; set; }
    public Race Race { get; set; }

    public List<Combat> Combats { get; set; }

    public Run() { }
}
