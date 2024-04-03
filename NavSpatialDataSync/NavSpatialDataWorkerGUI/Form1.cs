using NavSpatialDataWorker.DL;

namespace NavSpatialDataWorkerGUI
{
    public partial class Form1 : Form
    {

        string sourceConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=NavDatas;Integrated Security=True;";
        string destinationConnection = "Data Source=.\\SQLEXPRESS;Initial Catalog=NavSpatialData;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSyncAirports_Click(object sender, EventArgs e)
        {
            progressBarSync.Value = 0;

            AirportSync airportSync = new AirportSync(sourceConnection, destinationConnection);

            await Task.Run(() => { airportSync.SynchronizeAirports(); });

            progressBarSync.Value = 100;
            MessageBox.Show("Airports synchronization completed!");
        }

        private async void btnSyncWaypoints_Click(object sender, EventArgs e)
        {
            progressBarSync.Value = 0;
            EnrouteWaypointSync waypointSync = new EnrouteWaypointSync(sourceConnection, destinationConnection);
            await Task.Run(() => waypointSync.SynchronizeEnrouteWaypoints());

            progressBarSync.Value = 100;
            MessageBox.Show("Waypoints synchronization completed!");
        }
    }
}