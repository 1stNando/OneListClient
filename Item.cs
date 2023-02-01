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

        //This is a custom property.read only property to improve on True/False status of completion. We customized the get to be able to do the logic we want it to do. 
        public string CompletedStatus//we could make this a method instead and we can drop the get, and we call the method inside the {} 
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

                //Alternate great way: shorthand for if then else statement!
                // return complete ? "Completed" : "Not Completed";
            }

        }
    }
}