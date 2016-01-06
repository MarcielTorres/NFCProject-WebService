using System;
using NFCProject.Webservice.models;
using System.Collections.Generic;
using NFCProject.Webservice.helpers.storages;

namespace NFCProject.Webservice.entities
{
    public class PresenceDao : IDisposable
    {
        public PresenceControl ConfirmPresence(PresenceControl model)
        {
            using (Database db = new Database())
            {
                db.Open();

                //Valida email do inscrito no evento informado
                db.SqlStat.Append("SELECT ID FROM Subscribers WHERE EventID=@EventID ");
                db.SqlStat.Append("AND Mail=@Mail AND ID Not IN (SELECT SubscriberID FROM PresenceControl)");
                db.CreateParameter("@EventID", model.EventID);
                db.CreateParameter("@Mail", model.SubscriberMail);

                if (db.ExecuteDataReader())
                {
                    //Se não encontrou registros, então pode registrar presença
                    if (db.DataReader.HasRows)
                    {
                        db.DataReader.Read();
                        model.SubscriberID = Convert.ToString(db.DataReader["ID"]);

                        db.ClearParameters();
                        db.SqlStat.Clear();
                        db.DataReader.Close();

                        db.SqlStat.Append("INSERT INTO PresenceControl(EventID, SubscriberID, PresenceDate) VALUES (@EventID, @SubscriberID, @PresenceDate)");
                        db.CreateParameter("@EventID", model.EventID);
                        db.CreateParameter("@SubscriberID", model.SubscriberID);
                        db.CreateParameter("@PresenceDate", DateTime.Now);

                        if (!db.ExecuteNonQuery())
                        {
                            model = null;
                        }
                    }
                    else
                    {
                        model = null;
                    }
                }
                else
                {
                    model = null;
                }



            }

            return model;
        }

        public List<PresenceControl> LoadAll()
        {
            List<PresenceControl> _resp = new List<PresenceControl>();

            using (Database db = new Database())
            {
                db.Open();
                db.SqlStat.Append("SELECT ID, EventID, SubscriberID, PresenceDate, ");
                db.SqlStat.Append(" (SELECT TOP 1 Name FROM Subscribers WHERE ID = PresenceControl.SubscriberID) AS SubscriberName, ");
                db.SqlStat.Append(" (SELECT TOP 1 Title FROM Events WHERE ID = PresenceControl.EventID) AS EventName ");
                db.SqlStat.Append(" FROM PresenceControl ");
                db.SqlStat.Append("ORDER BY PresenceDate DESC");
                if (db.ExecuteDataReader())
                {
                    if (db.DataReader.HasRows)
                    {
                        while (db.DataReader.Read())
                        {
                            PresenceControl _model = new PresenceControl();
                            _model.ID = Convert.ToString(db.DataReader["ID"]);
                            _model.EventID = Convert.ToString(db.DataReader["EventID"]);
                            _model.EventName = Convert.ToString(db.DataReader["EventName"]);
                            _model.SubscriberID = Convert.ToString(db.DataReader["SubscriberID"]);
                            _model.SubscriberName = Convert.ToString(db.DataReader["SubscriberName"]);
                            _model.PresenceDate = Convert.ToString(db.DataReader["PresenceDate"]);

                            _resp.Add(_model);
                        }
                    }
                }

            }

            return _resp;
        }

        public PresenceControl Load(PresenceControl model)
        {
            using (Database db = new Database())
            {
                //Valida email do inscrito no evento informado
                db.SqlStat.Append("SELECT ID, Name, (SELECT Title FROM Events WHERE ID = Subscribers.EventID) AS EventName ");
                db.SqlStat.Append(" FROM Subscribers WHERE EventID=@EventID ");
                db.SqlStat.Append(" AND Mail=@Mail AND ID Not IN (SELECT SubscriberID FROM PresenceControl)");
                db.CreateParameter("@EventID", model.EventID);
                db.CreateParameter("@Mail", model.SubscriberMail);

                if (db.ExecuteDataReader())
                {
                    //Se não encontrou registros, então pode registrar presença
                    if (db.DataReader.HasRows)
                    {
                        db.DataReader.Read();
                        model.SubscriberID = Convert.ToString(db.DataReader["ID"]);
                        model.SubscriberName = Convert.ToString(db.DataReader["Name"]);
                        model.EventName = Convert.ToString(db.DataReader["EventName"]);
                    }
                    else
                    {
                        model = null;
                    }
                }
                else
                {
                    model = null;
                }
            }

            return model;
        }

        public bool Remove(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                using (Database db = new Database())
                {
                    db.Open();
                    db.SqlStat.Append("DELETE FROM PresenceControl WHERE ID=@ID;");
                    db.CreateParameter("@ID", Convert.ToInt32(id));

                    if (db.ExecuteNonQuery())
                    {
                        db.Close();
                        return true;
                    }
                }
            }
            
            return false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
