using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialDataWorker.DL
{
    public class AirwaysSync
    {
        private readonly string sourceConnection;
        private readonly string destinationConnection;

        public AirwaysSync(string sourceConn, string destConn)
        {
            sourceConnection = sourceConn;
            destinationConnection = destConn;
        }


        public void SynchronizeAirways()
        {


            using (SqlConnection srcConn = new SqlConnection(sourceConnection), destConn = new SqlConnection(destinationConnection))
            {
                srcConn.Open();
                destConn.Open();
                SqlTransaction transaction = destConn.BeginTransaction();
                try
                {
                    AddNewAirways(srcConn, destConn, transaction);
                    DeleteObsoleteAirways(srcConn,destConn, transaction);
                    UpdateExistingAirways(srcConn, destConn, transaction);

                    transaction.Commit();
                    Console.WriteLine("Synchronization of Airways completed successfully.");
                    Console.WriteLine();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}. Rolling back transaction.");
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        Console.WriteLine($"An error occurred while trying to roll back the transaction: {exRollback.Message}");
                    }
                }
            }
        }

        private void AddNewAirways(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            // SQL command that add new airway points segments from the destination database to the new database 

            string sql = @"
                        INSERT INTO NavSpatialData.Nav.EnrouteAirwaysLines
                        (CycleId, AreaCode, RouteIdentifier, SequenceNumber, IcaoCode, WaypointId, 
                         WaypointDescriptionCode, BoundaryCode, RouteType, Level, DirectionRestriction, 
                         CruiseTableIndicator, EUIndicator, RecommendedNavaid, RNP, Theta, Rho, 
                         OutboundMagneticCourse, InboundMagneticCourse, RouteDistanceFrom, DistanceTime, 
                         MinimumAltitude1, MinimumAltitude2, MaximumAltitude, FixRadiusTransitionIndicator, 
                         CycleDate, WaypointLatitude, WaypointLongitude, WaypointSource, FIRIdentifier, 
                         UIRIdentifier, DmsLatitude, DmsLongitude)
                        SELECT src.CycleId, src.AreaCode, src.RouteIdentifier, src.SequenceNumber, src.IcaoCode, 
                               src.WaypointId, src.WaypointDescriptionCode, src.BoundaryCode, src.RouteType, 
                               src.Level, src.DirectionRestriction, src.CruiseTableIndicator, src.EUIndicator, 
                               src.RecommendedNavaid, src.RNP, src.Theta, src.Rho, src.OutboundMagneticCourse, 
                               src.InboundMagneticCourse, src.RouteDistanceFrom, src.DistanceTime, src.MinimumAltitude1, 
                               src.MinimumAltitude2, src.MaximumAltitude, src.FixRadiusTransitionIndicator, src.CycleDate, 
                               src.WaypointLatitude, src.WaypointLongitude, src.WaypointSource, src.FIRIdentifier, 
                               src.UIRIdentifier, src.DmsLatitude, src.DmsLongitude
                        FROM NavDatas.Nav.EnrouteAirway src
                        LEFT JOIN NavSpatialData.Nav.EnrouteAirwaysLines dest
                        ON src.CycleId = dest.CycleId AND src.AreaCode = dest.AreaCode 
                           AND src.RouteIdentifier = dest.RouteIdentifier AND src.SequenceNumber = dest.SequenceNumber
                        WHERE dest.CycleId IS NULL AND src.WaypointLatitude IS NOT NULL AND src.WaypointLongitude IS NOT NULL;
                    ";

            using (SqlCommand command = new SqlCommand(sql, destConn, transaction))
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} new airway segments have been added to the destination database.");
            }
        }

        private void DeleteObsoleteAirways(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            // SQL command that deletes obsolete airway segments from the destination database

            string sql = @"
                        DELETE dest
                        FROM NavSpatialData.Nav.EnrouteAirwaysLines dest
                        LEFT JOIN NavDatas.Nav.EnrouteAirway src
                        ON dest.CycleId = src.CycleId 
                           AND dest.AreaCode = src.AreaCode 
                           AND dest.RouteIdentifier = src.RouteIdentifier 
                           AND dest.SequenceNumber = src.SequenceNumber
                        WHERE src.CycleId IS NULL;";

            using (SqlCommand command = new SqlCommand(sql, destConn, transaction))
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} obsolete airway segments have been deleted from the destination database.");
            }
        }

        private void UpdateExistingAirways(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            Console.WriteLine($"Using database: {destConn.Database}");

            string sql = @"
                        UPDATE dest
                        SET 
                            dest.IcaoCode = src.IcaoCode,
                            dest.WaypointId = src.WaypointId,
                            dest.WaypointDescriptionCode = src.WaypointDescriptionCode,
                            dest.BoundaryCode = src.BoundaryCode,
                            dest.RouteType = src.RouteType,
                            dest.Level = src.Level,
                            dest.DirectionRestriction = src.DirectionRestriction,
                            dest.CruiseTableIndicator = src.CruiseTableIndicator,
                            dest.EUIndicator = src.EUIndicator,
                            dest.RecommendedNavaid = src.RecommendedNavaid,
                            dest.RNP = src.RNP,
                            dest.Theta = src.Theta,
                            dest.Rho = src.Rho,
                            dest.OutboundMagneticCourse = src.OutboundMagneticCourse,
                            dest.InboundMagneticCourse = src.InboundMagneticCourse,
                            dest.RouteDistanceFrom = src.RouteDistanceFrom,
                            dest.DistanceTime = src.DistanceTime,
                            dest.MinimumAltitude1 = src.MinimumAltitude1,
                            dest.MinimumAltitude2 = src.MinimumAltitude2,
                            dest.MaximumAltitude = src.MaximumAltitude,
                            dest.FixRadiusTransitionIndicator = src.FixRadiusTransitionIndicator,
                            dest.CycleDate = src.CycleDate,
                            dest.WaypointLatitude = src.WaypointLatitude,
                            dest.WaypointLongitude = src.WaypointLongitude,
                            dest.WaypointSource = src.WaypointSource,
                            dest.FIRIdentifier = src.FIRIdentifier,
                            dest.UIRIdentifier = src.UIRIdentifier,
                            dest.DmsLatitude = src.DmsLatitude,
                            dest.DmsLongitude = src.DmsLongitude
                        FROM NavSpatialData.Nav.EnrouteAirwaysLines dest
                        INNER JOIN NavDatas.Nav.EnrouteAirway src
                            ON dest.CycleId = src.CycleId 
                            AND dest.AreaCode = src.AreaCode 
                            AND dest.RouteIdentifier = src.RouteIdentifier 
                            AND dest.SequenceNumber = src.SequenceNumber
                        WHERE src.IcaoCode != dest.IcaoCode
                            OR src.WaypointId != dest.WaypointId
                            OR src.WaypointDescriptionCode != dest.WaypointDescriptionCode
                            OR src.BoundaryCode != dest.BoundaryCode
                            OR src.RouteType != dest.RouteType
                            OR src.Level != dest.Level
                            OR src.DirectionRestriction != dest.DirectionRestriction
                            OR src.CruiseTableIndicator != dest.CruiseTableIndicator
                            OR src.EUIndicator != dest.EUIndicator
                            OR src.RecommendedNavaid != dest.RecommendedNavaid
                            OR src.RNP != dest.RNP
                            OR src.Theta != dest.Theta
                            OR src.Rho != dest.Rho
                            OR src.OutboundMagneticCourse != dest.OutboundMagneticCourse
                            OR src.InboundMagneticCourse != dest.InboundMagneticCourse
                            OR src.RouteDistanceFrom != dest.RouteDistanceFrom
                            OR src.DistanceTime != dest.DistanceTime
                            OR src.MinimumAltitude1 != dest.MinimumAltitude1
                            OR src.MinimumAltitude2 != dest.MinimumAltitude2
                            OR src.MaximumAltitude != dest.MaximumAltitude
                            OR src.FixRadiusTransitionIndicator != dest.FixRadiusTransitionIndicator
                            OR src.CycleDate != dest.CycleDate
                            OR src.WaypointLatitude != dest.WaypointLatitude
                            OR src.WaypointLongitude != dest.WaypointLongitude
                            OR src.WaypointSource != dest.WaypointSource
                            OR src.FIRIdentifier != dest.FIRIdentifier
                            OR src.UIRIdentifier != dest.UIRIdentifier
                            OR src.DmsLatitude != dest.DmsLatitude
                            OR src.DmsLongitude != dest.DmsLongitude;
                    ";

            using (SqlCommand command = new SqlCommand(sql, destConn, transaction))
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} existing airway segments have been updated in the destination database.");
            }
        }


    }
}
