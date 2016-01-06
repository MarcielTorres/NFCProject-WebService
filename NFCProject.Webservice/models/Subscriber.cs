namespace NFCProject.Webservice.models
{
    public class Subscriber
    {
        public Subscriber()
        {
        }

        public Subscriber(string _id, string _eventID, string _name, string _mail, string _eventName, string _presenceDate)
        {
            this.ID = _id;
            this.EventID = _eventID;
            this.Name = _name;
            this.Mail = _mail;
            this.EventName = _eventName;
            this.PresenceDate = _presenceDate;
        }

        public string ID { get; set; }
        public string EventID { get; set; }
        public string EventName { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string PresenceDate { get; set; }
    }
}
