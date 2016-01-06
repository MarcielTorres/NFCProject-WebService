using System;
using System.Collections.Generic;
using NFCProject.Webservice.entities._default;
using NFCProject.Webservice.helpers.storages;
using NFCProject.Webservice.models;

namespace NFCProject.Webservice.entities
{
    public class SubscriberDao : IDefaultDao<Subscriber>
    {
        public Subscriber Load(object id)
        {
            Subscriber model = new Subscriber();
            using (Database db = new Database())
            {
                db.Open();
                db.SqlStat.Append(" SELECT ID, Name, Mail, EventID, ");
                db.SqlStat.Append(" (SELECT TOP 1 Title FROM Events WHERE ID = Subscribers.EventID) AS EventName ");
                db.SqlStat.Append(" FROM Subscribers ");

                if (id != null)
                {

                    db.SqlStat.Append(" WHERE ID = @ID ORDER BY Name ");
                    db.CreateParameter("@ID", Convert.ToInt16(id));

                    if (db.ExecuteDataReader())
                    {
                        if (db.DataReader.Read())
                        {
                            model.ID = Convert.ToString(db.DataReader["ID"]);
                            model.Name = Convert.ToString(db.DataReader["Name"]);
                            model.Mail = Convert.ToString(db.DataReader["Mail"]);
                            model.EventID = Convert.ToString(db.DataReader["EventID"]);
                            model.EventName = Convert.ToString(db.DataReader["EventName"]);
                        }
                    }
                }
                db.Close();
            }

            return model;
        }

        public List<Subscriber> LoadAll()
        {
            List<Subscriber> _list = new List<Subscriber>();

            using (Database db = new Database())
            {
                db.Open();
                db.SqlStat.Append(" SELECT ID, Name, Mail, EventID, ");
                db.SqlStat.Append(" (SELECT TOP 1 Title FROM Events WHERE ID = Subscribers.EventID) AS EventName, ");
                db.SqlStat.Append(" (SELECT TOP 1 PresenceDate FROM PresenceControl WHERE EventID = Subscribers.EventID AND SubscriberID = Subscribers.ID ) AS PresenceDate ");
                db.SqlStat.Append(" FROM Subscribers ORDER BY Name ");

                if (db.ExecuteDataReader())
                {
                    if (db.DataReader.HasRows)
                    {
                        while (db.DataReader.Read())
                        {
                            Subscriber _model = new Subscriber();
                            _model.ID = Convert.ToString(db.DataReader["ID"]);
                            _model.Name = Convert.ToString(db.DataReader["Name"]);
                            _model.Mail = Convert.ToString(db.DataReader["Mail"]);
                            _model.EventID = Convert.ToString(db.DataReader["EventID"]);
                            _model.EventName = Convert.ToString(db.DataReader["EventName"]);
                            _model.PresenceDate = Convert.ToString(db.DataReader["PresenceDate"]);
                            _list.Add(_model);
                        }
                    }
                }
            }

            return _list;
        }

        public bool Remove(Subscriber model)
        {
            if (model != null)
            {
                using (Database db = new Database())
                {
                    db.Open();
                    db.SqlStat.Append("DELETE FROM PresenceControl WHERE SubscriberID=@SubscriberID;");
                    db.SqlStat.Append("DELETE FROM Subscribers WHERE ID=@ID;");
                    db.CreateParameter("@ID", model.ID);
                    db.CreateParameter("@SubscriberID", model.ID);

                    if (db.ExecuteNonQuery())
                    {
                        db.Close();
                        return true;
                    }
                }
            }
            return false;
        }

        public Subscriber Save(Subscriber model)
        {
            if (model != null)
            {
                //Determina se é uma edição
                bool _edit = !String.IsNullOrEmpty(model.ID);

                using (Database db = new Database())
                {
                    db.Open();

                    //verifica se não existe inscrição para o evento com o e-mail informado
                    db.SqlStat.Append("SELECT ID FROM Subscribers WHERE EventID=@_EventID AND Mail=@_Mail");
                    db.CreateParameter("@_Mail", model.Mail);
                    db.CreateParameter("@_EventID", Convert.ToInt32(model.EventID));

                    if (db.ExecuteDataReader())
                    {
                        if (!db.DataReader.HasRows)
                        {
                            db.ClearParameters();
                            db.SqlStat.Clear();
                            db.DataReader.Close();

                            db.CreateParameter("@Name", model.Name);
                            db.CreateParameter("@Mail", model.Mail);
                            db.CreateParameter("@EventID", Convert.ToInt32(model.EventID));
                            
                            db.SqlStat.Append("INSERT INTO Subscribers(Name, Mail, EventID) VALUES(@Name, @Mail, @EventID) SELECT @@IDENTITY");
                            model.ID = Convert.ToString(db.ExecuteScalar());
                            model = String.IsNullOrEmpty(model.ID) ? null : model;
                            
                            db.Close();
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
            }
            return model;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
