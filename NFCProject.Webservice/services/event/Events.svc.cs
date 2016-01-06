using System;
using NFCProject.Webservice.response.events;
using NFCProject.Webservice.models;
using NFCProject.Webservice.entities;
using System.ServiceModel.Activation;
using System.ServiceModel;

namespace NFCProject.Webservice.services
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Events : IEvents
    {
        public ResponseEvent CreateEvent(Event model)
        {
            ResponseEvent _resp = new ResponseEvent();
            
            //model.EndDate = DateTime.Now;
            //model.StartDate = DateTime.Now;

            using (EventDao _eventDao = new EventDao())
            {
                model = _eventDao.Save(model);

                if (model != null)
                {
                    _resp.Status.Success = "1";
                    _resp.Status.Code = "SUCCESS-EVENT-OO1";
                    _resp.Status.Message = "Evento Adicionado com sucesso.";
                    _resp.Event = model;
                }
                else
                {
                    _resp.Status.Success = "0";
                    _resp.Status.Code = "FAIL-EVENT-OO1";
                    _resp.Status.Message = "Não foi possível adicionar o evento.";
                }
            }            

            return _resp;
        }

        public ResponseEvent EditEvent(string _title, string _description, string _startDate, string _endDate)
        {
            throw new NotImplementedException();
        }

        public ResponseEvents ListEvents()
        {
            ResponseEvents _resp = new ResponseEvents();
            using (EventDao _eventDao = new EventDao())
            {
                _resp.Events = _eventDao.LoadAll();
            }
            
            return _resp;
        }

        public ResponseEvent RemoveEvent(string _id)
        {
            ResponseEvent _resp = new ResponseEvent();
            Event _event = new Event();

            using (EventDao _eventDao = new EventDao())
            {
                _event = _eventDao.Load(Convert.ToInt16(_id));

                if (_eventDao.Remove(_event))
                {
                    _resp.Status.Success = "1";
                    _resp.Status.Code = "SUCESS-EVENT-OO1";
                    _resp.Status.Message = "Evento removido com sucesso.";
                    _resp.Event = _event;
                }
                else
                {
                    _resp.Status.Success = "0";
                    _resp.Status.Code = "FAIL-EVENT-OO1";
                    _resp.Status.Message = "Não foi possível remover o evento.";
                }
            }
            
            return _resp;
        }

        public ResponseEvent ShowEvent(string _id)
        {
            ResponseEvent _resp = new ResponseEvent();
            Event _event = new Event();

            using (EventDao _eventDao = new EventDao())
            {
                _event = _eventDao.Load(Convert.ToInt16(_id));

                if (_event!= null)
                {
                    _resp.Status.Success = "1";
                    _resp.Status.Code = "SUCCESS-EVENT-OO1";
                    _resp.Status.Message = "Evento Encontrado";
                    _resp.Event = _event;
                }
                else
                {
                    _resp.Status.Success = "0";
                    _resp.Status.Code = "FAIL-EVENT-OO1";
                    _resp.Status.Message = "Não foi possível remover o evento.";
                }
            }

            return _resp;
        }
    }
}
