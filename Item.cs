using System;

namespace OneListClient
{
    public class Item
    {
        public int id { get; set; }
        public string text { get; set; }
        public bool complete { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //read only property to improve on True/False status of completion. We customized the get to be able to do the logic we want it to do. 
        public string CompletedStatus
        {
            get
            {
                if (complete == true)
                {
                    return "Completed";
                }
                else
                {
                    return "Not Completed";
                }
            }

        }
    }
}