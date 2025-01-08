#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.ODBCStore;
using FTOptix.Store;
#endregion

public class NetLogicDialogPrep : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        string eventID = Owner.GetVariable("sEventID").Value;
        // Get the Database from the current project
        var myStore = Project.Current.Get<Store>("DataStores/FTAEDatabase");
        // Create the output to get the result (mandatory)
        Object[,] ResultSet;
        String[] Header;
        string query = "SELECT * FROM AllEventComments WHERE sEventID = '" + eventID + "'";
        // Perform the query
        myStore.Query(query, out Header, out ResultSet);
        
        //If no result, this is a new record that needs inserted into the dbo.AllEventComments table
        if (ResultSet.GetLength(0)==0){
            string[] columns = {"sEventID", "sComment1", "sComment2"};
            object[,] rawValues = new object[1,3];
            rawValues[0,0] = eventID;
            rawValues[0,1] = "";
            rawValues[0,2] = "";
            var allEventCommentsTable = myStore.Tables.Get<Table>("AllEventComments");
            allEventCommentsTable.Insert(columns, rawValues);
            Log.Info("Inserted new row in AllEventComments. EventID: "+eventID);
        }
        else{
            var sComment1 = Owner.GetVariable("sComment1");
            var sComment2 = Owner.GetVariable("sComment2");
            sComment1.Value = (string)ResultSet[0, 1];
            sComment2.Value = (string)ResultSet[0, 2];
        }
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
