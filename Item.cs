using System;
using System.Text.Json.Serialization;

namespace OneListClient
{
    public class Item
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("complete")]
        public bool Complete { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        //This is a custom property.read only property to improve on True/False status of completion. We customized the get to be able to do the logic we want it to do. 
        public string CompletedStatus//we could make this a method instead and we can drop the get, and we call the method inside the {} 
        {
            get
            {
                //Alternate great way: shorthand for if then else statement!
                return Complete ? "Completed" : "Not Completed";

                // if (Complete == true)
                // {
                //     return "Completed";
                // }
                // else
                // {
                //     return "Not Completed";
                // }
            }

        }
    }
}