using NavSpatialDataWorker.DL;

namespace NavSpatialDataWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=NavDatas;Integrated Security=True;";
            string destinationConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=NavSpatialData;Integrated Security=True;";


            Console.WriteLine("Starting synchrnization");
            Console.WriteLine();

            AirportSync airportSync = new AirportSync(sourceConnection, destinationConnection);
            //airportSync.SynchronizeAirports();

            VhfNavaidSync vhfNavaidSync = new VhfNavaidSync(sourceConnection, destinationConnection);
            //vhfNavaidSync.SynchronizeVhfNavaids();

            EnrouteWaypointSync enrouteWaypointAsync = new EnrouteWaypointSync(sourceConnection, destinationConnection);
            //enrouteWaypointAsync.SynchronizeEnrouteWaypoints();

            AirwaysSync airwaysSync = new AirwaysSync(sourceConnection, destinationConnection);
            //airwaysSync.SynchronizeAirways();

            FirUirSync firUirSync = new FirUirSync(sourceConnection, destinationConnection);
            firUirSync.SynchronizeFirUir();
            //SynchronizeAirports();
            //SynchronizeVhfNavaids();
            //SynchronizeEnrouteWaypoints();
            Console.WriteLine("finished synchrnization");
        }
    }
}