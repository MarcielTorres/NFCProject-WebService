using NFCProject.Webservice.models;
using NFCProject.Webservice.response.presences;
using NFCProject.Webservice.entities;
using System;

namespace NFCProject.Webservice.services.presences
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Presence" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Presence.svc or Presence.svc.cs at the Solution Explorer and start debugging.
    public class Presence : IPresence
    {
        public ResponsePresences ConfirmPresence(PresenceControl Model)
        {
            ResponsePresences _resp = new ResponsePresences();
            _resp.Status.Success = "0";
            _resp.Status.Message = "Erro não definido";
            _resp.Status.Code = "0";

            using (PresenceDao _presenceDao = new PresenceDao())
            {
                PresenceControl _model = _presenceDao.ConfirmPresence(Model);

                if (_model != null)
                {
                    _resp.Presences.Add(_model);
                    _resp.Status.Success = "1";
                    _resp.Status.Message = "Presença confirmada com sucesso";
                    _resp.Status.Code = "SUCCESS-001";
                }
                else
                {
                    _resp.Status.Success = "0";
                    _resp.Status.Message = "Não foi possível registrar a presença";
                    _resp.Status.Code = "ERROR-001";
                }
            }

            return _resp;
        }

        public ResponsePresences ListPresences()
        {
            ResponsePresences _resp = new ResponsePresences();

            using (PresenceDao _presenceDao = new PresenceDao())
            {
                _resp.Presences = _presenceDao.LoadAll();
            }

            return _resp;
        }

        public ResponsePresences RemovePresence(string id)
        {
            ResponsePresences _resp = new ResponsePresences();
            _resp.Status.Success = "0";
            _resp.Status.Message = "Não foi possível remover o registro de presença";
            _resp.Status.Code = "ERROR-REMOVE-001";

            using (PresenceDao _presenceDao = new PresenceDao())
            {
                if (_presenceDao.Remove(id))
                {
                    _resp.Status.Success = "1";
                    _resp.Status.Message = "registro de presença removido com sucesso";
                    _resp.Status.Code = "SUCCESS-REMOVE-001";
                }
            }

            return _resp;
        }

        public ResponsePresences VerifySubscriber(PresenceControl Model)
        {
            ResponsePresences _resp = new ResponsePresences();
            _resp.Status.Success = "0";
            _resp.Status.Message = "Erro não definido";
            _resp.Status.Code = "0";

            using (PresenceDao _presenceDao = new PresenceDao())
            {
                PresenceControl _model = new PresenceControl();
                _model = _presenceDao.Load(Model);

                if (_model != null)
                {
                    _resp.Presences.Add(_model);
                    _resp.Status.Success = "1";
                    _resp.Status.Message = "Inscrito encontrado";
                    _resp.Status.Code = "SUCCESS-VERIFY-OO1";
                }
                else
                {
                    _resp.Status.Success = "0";
                    _resp.Status.Message = "Inscrito não encontrado";
                    _resp.Status.Code = "ERROR-VERIFY-OO1";
                }
            }

            return _resp;
        }
    }
}
