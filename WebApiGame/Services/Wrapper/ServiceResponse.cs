using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGame.Services
{
    // NOTE: ServiceResponse is a wrapper class, we wrap the return value of functions on ICharacterService using this class
    //The main reason of creating this class is for when the project gets bigger, we can get and set values other than the main data
    //like if the operation was succesful or any message about execution. This helps in creating proper UI states as well.

    // further note, if we had not used the wrapper class we could have performed mapping in controller itself, 
    public class ServiceResponse<T>
    {
        public T data { get; set; }
        public bool success { get; set; } = true;
        public string message { get; set; } = null;
    }
}
