using System.Collections.Generic;

namespace AppBlogEngine.Models
{
    public class StatePublished
    {
        public int value { get; set; }
        public string nameState { get; set; }

        public List<StatePublished> ListStates()
        {
            List<StatePublished> statePublishedL = new List<StatePublished>()
            {
                new StatePublished() { value = 0, nameState = "Created" },
                new StatePublished() { value = 1, nameState = "Send" },
                new StatePublished() { value = 2, nameState = "Approved" },
                new StatePublished() { value = 3, nameState = "Reyected" }
            };

            return statePublishedL; 
        }
    }
}
