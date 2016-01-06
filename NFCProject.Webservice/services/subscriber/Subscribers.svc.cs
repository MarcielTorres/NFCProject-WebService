using System;
using NFCProject.Webservice.response.subscribers;
using NFCProject.Webservice.models;
using NFCProject.Webservice.entities;
using System.Collections.Generic;

namespace NFCProject.Webservice.services.subscriber
{
    public class Subscribers : ISubscribers
    {
        public ResponseSubscriber CreateSubscriber(Subscriber model)
        {
            ResponseSubscriber _resp = new ResponseSubscriber();

            using (SubscriberDao _subsDao = new SubscriberDao())
            {
                model = _subsDao.Save(model);
            }

            if (model != null)
            {
                _resp.Status.Success = "1";
                _resp.Status.Message = "Inscrito cadastrado com sucesso";
                _resp.Status.Code = "SUCCESS-SUBSCRIBER-001";
            }
            else
            {
                _resp.Status.Success = "0";
                _resp.Status.Message = "Não possível registrar o inscrito";
                _resp.Status.Code = "ERROR-SUBSCRIBER-001";
            }

            _resp.Subscriber = model;

            return _resp;
        }

        public ResponseSubscriber EditSubscriber(Subscriber model)
        {
            ResponseSubscriber _resp = new ResponseSubscriber();

            using (SubscriberDao _subsDao = new SubscriberDao())
            {
                model = _subsDao.Load(model.ID);

                if (model != null)
                {
                    model = _subsDao.Save(model);
                    _resp.Subscriber = model;
                }
            }

            return _resp;
        }

        public ResponseSubscribers ListSubscribers()
        {
            ResponseSubscribers _resp = new ResponseSubscribers();

            using (SubscriberDao _subsDao = new SubscriberDao())
            {
                _resp.Subscribers = _subsDao.LoadAll();
            }
            
            return _resp;
        }

        public ResponseSubscriber RemoveSubscriber(string _id)
        {
            ResponseSubscriber _resp = new ResponseSubscriber();
            Subscriber _subs = new Subscriber();

            using (SubscriberDao _subsDao = new SubscriberDao())
            {
                _subs = _subsDao.Load(_id);

                _subsDao.Remove(_subs);
            }

            return _resp;
        }

        public ResponseSubscriber ShowSubscriber(string _id)
        {
            ResponseSubscriber _resp = new ResponseSubscriber();
            Subscriber _subs = new Subscriber();


            using (SubscriberDao _subsDao = new SubscriberDao())
            {
                _subs = _subsDao.Load(_id);
                _resp.Subscriber = _subs;
            }

            return _resp;
        }
    }
}
