public class PlaceController
{
    private Hub _hub;    

    public PlaceController(Hub hub)
    {
        _hub = hub;
    }

    public int Place
    {
        get
        {
            int output = 1;
            int playerPoints = _hub.Level.Race.Car.LapsCounter.Points;

            foreach (Car enemy in _hub.Level.Race.Enemies) 
            { 
                if (enemy.LapsCounter.Points > playerPoints)
                    output++;

                if (enemy.LapsCounter.Points == playerPoints)
                {
                    //TODO: 
                    output++;
                }
            }

            return output;
        }
    }
}
