using NFCProject.Webservice.entities._default;
using NFCProject.Webservice.helpers.storages;
using NFCProject.Webservice.models;
using System;
using System.Collections.Generic;

namespace NFCProject.Webservice.entities
{
    public class EventDao : IDefaultDao<Event>
    {
        public Event Load(object id)
        {
            Event model = new Event();
            using (Database db = new Database())
            {
                db.Open();
                db.SqlStat.Append(" SELECT ID, Title, Description, StartDate, EndDate FROM Events ");

                if (id != null)
                {
                    db.CreateParameter("@ID", Convert.ToString(id));
                    db.SqlStat.Append(" WHERE ID = @ID ");

                    if (db.ExecuteDataReader())
                    {
                        if (db.DataReader.Read())
                        {
                            model.ID = Convert.ToString(db.DataReader["ID"]);
                            model.Title = Convert.ToString(db.DataReader["Title"]);
                            model.Description = Convert.ToString(db.DataReader["Description"]);
                            model.StartDate = Convert.ToString(db.DataReader["StartDate"] );
                            model.EndDate = Convert.ToString(db.DataReader["EndDate"]);
                        }
                    }
                }
                db.Close();
            }

            return model;
        }

        public List<Event> LoadAll()
        {
            List<Event> _list = new List<Event>();

            using (Database db = new Database())
            {
                db.Open();
                db.SqlStat.Append("SELECT ID, Title, Description, StartDate, EndDate, ");
                db.SqlStat.Append("(SELECT COUNT(ID) FROM Subscribers WHERE EventID = Events.ID) AS SubscribersNumber, ");
                db.SqlStat.Append("(SELECT COUNT(ID) FROM PresenceControl WHERE EventID = Events.ID) AS SubscribersPresences ");
                db.SqlStat.Append(" FROM Events ");
                db.SqlStat.Append(" ORDER BY StartDate DESC ");

                if (db.ExecuteDataReader())
                {
                    if (db.DataReader.HasRows)
                    {
                        while (db.DataReader.Read())
                        {
                            Event _model = new Event();
                            _model.ID = Convert.ToString(db.DataReader["ID"]);
                            _model.Title = Convert.ToString(db.DataReader["Title"]);
                            _model.Description = Convert.ToString(db.DataReader["Description"]);
                            _model.StartDate = Convert.ToString(db.DataReader["StartDate"]);
                            _model.EndDate = Convert.ToString(db.DataReader["EndDate"]);
                            _model.SubscribersNumber = Convert.ToString(db.DataReader["SubscribersNumber"]);
                            _model.SubscribersPresences = Convert.ToString(db.DataReader["SubscribersPresences"]);
                            _list.Add(_model);
                        }
                    }
                }
            }

            return _list;
        }

        public Event Save(Event model)
        {
            if (model != null)
            {
                //Determina se é uma edição
                bool _edit = !String.IsNullOrEmpty(model.ID);

                using (Database db = new Database())
                {
                    DateTime _start;
                    if (DateTime.TryParse(model.StartDate, out _start))
                    {
                        model.StartDate = _start.ToString();
                    }
                    else
                    {
                        model.StartDate = DateTime.Now.ToString();
                    }

                    DateTime _end;
                    if (DateTime.TryParse(model.StartDate, out _end))
                    {
                        model.EndDate = _end.ToString();
                    }
                    else
                    {
                        model.EndDate = DateTime.Now.ToString();
                    }

                    db.Open();
                    db.CreateParameter("@Title", model.Title);
                    db.CreateParameter("@Description", model.Description);
                    db.CreateParameter("@StartDate", Convert.ToDateTime(model.StartDate));
                    db.CreateParameter("@EndDate", Convert.ToDateTime(model.EndDate));

                    if (!_edit)
                    {
                        db.SqlStat.Append("INSERT INTO Events(Title, Description, StartDate, EndDate) VALUES(@Title, @Description, @StartDate, @EndDate) SELECT @@IDENTITY");
                        model.ID = Convert.ToString(db.ExecuteScalar());
                        model = String.IsNullOrEmpty(model.ID) ? null : model;
                    }
                    else
                    {
                        db.SqlStat.Append("UPDATE Events SET Title = @Title, Description = @Description, StartDate = @StartDate, EndDate = @EndDate WHERE ID = @ID");
                        db.CreateParameter("@ID", model.ID);
                        model = db.ExecuteNonQuery() ? model : null;
                    }
                    db.Close();
                }
            }
            return model;
        }

        public bool Remove(Event model)
        {
            if (model != null)
            {
                using (Database db = new Database())
                {
                    db.Open();
                    db.SqlStat.Append("DELETE FROM PresenceControl WHERE EventID=@ID;");
                    db.SqlStat.Append("DELETE FROM Subscribers WHERE EventID=@ID;");
                    db.SqlStat.Append("DELETE FROM Events WHERE ID=@ID;");
                    db.CreateParameter("@ID", model.ID);

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
