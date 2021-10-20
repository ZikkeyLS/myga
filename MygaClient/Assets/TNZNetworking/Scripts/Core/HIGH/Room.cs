public class Room
{
    public string name { get; private set; }
    public int maxPlayers { get; private set; }
    public int currentPlayers { get; private set; }

    public void Initialise(string name, int maxPlayers, int currentPlayers)
    {
        this.name = name;
        this.maxPlayers = maxPlayers;
        this.currentPlayers = currentPlayers;
    }
}