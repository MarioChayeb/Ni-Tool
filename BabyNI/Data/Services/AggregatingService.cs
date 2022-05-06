    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Odbc;
using Microsoft.Extensions.Configuration;

namespace BabyNI.Data.Services
{
    public class AggregatingService
    {
        private readonly IConfiguration configuration;

        public AggregatingService(IConfiguration Configuration)
        {
            configuration = Configuration;
        }

        public void AggregateData()
        {
          
            string connectionString = configuration.GetConnectionString("VerticaConnectionString");



            //////////////////////////////////////////////////////////////////HOURLY AGGREGATION//////////////////////////////////////////////////////////////////
            string AggregateHourly = "Insert into TRANS_MW_AGG_SLOT_HOURLY (TIME ,LINK,NEALIAS,NETYPE,MAXRXLEVEL,MAXTXLEVEL,MAX_RX_LEVEL_db,MAX_TX_LEVEL_db,RSL_DEVIATION_db)" +
                " SELECT DATE_TRUNC('HOUR', TIME),LINK,NEALIAS,NETYPE,MAXRXLEVEL,MAXTXLEVEL,MAX(MAXRXLEVEL),MAX(MAXTXLEVEL),MAX(ABS(MAXRXLEVEL) - ABS(MAXTXLEVEL))FROM TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER" +
                " GROUP BY DATE_TRUNC('HOUR', TIME),LINK,NEALIAS,NETYPE,MAXRXLEVEL,MAXTXLEVEL";

            //////////////////////////////////////////////////////////////////HOURLY AGGREGATION//////////////////////////////////////////////////////////////////




            //////////////////////////////////////////////////////////////////DAILU AGGREGATION//////////////////////////////////////////////////////////////////

            string AggregateDaily = "Insert into TRANS_MW_AGG_SLOT_DAILY(TIME,LINK,NEALIAS,NETYPE,MAXRXLEVEL,MAXTXLEVEL,MAX_RX_LEVEL_db,MAX_TX_LEVEL_db,RSL_DEVIATION_db)" +
                " SELECT DATE_TRUNC('DAY', TIME),LINK,NEALIAS,NETYPE,MAXRXLEVEL,MAXTXLEVEL,MAX_RX_LEVEL_db,MAX_TX_LEVEL_db,RSL_DEVIATION_db" +
                " FROM TRANS_MW_AGG_SLOT_HOURLY" +
                " GROUP BY DATE_TRUNC('DAY',TIME),LINK,NEALIAS,NETYPE,MAXRXLEVEL,MAXTXLEVEL,MAX_RX_LEVEL_db,MAX_TX_LEVEL_db,RSL_DEVIATION_db";

            //////////////////////////////////////////////////////////////////DAILY AGGREGATION//////////////////////////////////////////////////////////////////


            OdbcCommand dailyagregatorcommand = new OdbcCommand(AggregateDaily);
            OdbcCommand hourlyagregatorcommand = new OdbcCommand(AggregateHourly);
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
              
                hourlyagregatorcommand.Connection = conn;
                conn.Open();
                hourlyagregatorcommand.ExecuteNonQuery();
                dailyagregatorcommand.Connection = conn;
                dailyagregatorcommand.ExecuteNonQuery();
                conn.Close();
            }
         
        }



    }
}
