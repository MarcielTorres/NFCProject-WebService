using NFCProject.Webservice.entities;
using NFCProject.Webservice.models;
using System;
using System.Collections.Generic;

namespace NFCProject.Webservice
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Event> _list = new List<Event>();
            using (EventDao _eventDao = new EventDao())
            {
                _list = _eventDao.LoadAll();
            }

            foreach (var item in _list)
            {
                Response.Write("title:" + item.Title);

            }
        }
    }
}