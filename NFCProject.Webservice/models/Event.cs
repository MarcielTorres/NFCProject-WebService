using System;

namespace NFCProject.Webservice.models
{
   public class Event
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SubscribersNumber { get; set; }

        public string SubscribersPresences { get; set; }

    }

}
