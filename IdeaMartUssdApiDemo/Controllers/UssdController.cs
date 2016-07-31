using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nanosoft.IdeaMartAPI;
using System.Threading.Tasks;

namespace IdeaMartUssdApiDemo.Controllers
{
    public class UssdController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Send(IdeaMartResponse ideaMartresponse)
        {
            //there are two constructors available
            //we will use the simplest one here
            //Only app id and the password sent as params
            UssdAPI ussdApi = new UssdAPI("APP_000001", "password");
            //when you are ready to go with production change below to true
            ussdApi.IsInProduction = false;
            //we check first if the request from ideamart is initiation
            if (ideaMartresponse.ussdOperation == "mo-init")
            {
                //supprissed ? 
                //Yes! its that simple to check the operation type

                //All right
                //here comes the magical part
                IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
                statusResponse = await ussdApi.SendRequestAsync("Hello world! \r\n 1. Press one to do something.", ideaMartresponse.sessionId, UssdAPI.UssdOperation.mt_cont, ideaMartresponse.sourceAddress);
                return Ok(statusResponse);
            }

            //Happy?
            //let's implement continue operation
            if (ideaMartresponse.ussdOperation == "mo-cont")
            {
                switch (ideaMartresponse.message)
                {
                    case "1":
                        IdeaMartStatusResponse statusresponse = new IdeaMartStatusResponse();
                        statusresponse = await ussdApi.SendRequestAsync("You pressed one", ideaMartresponse.sessionId, UssdAPI.UssdOperation.mt_cont, ideaMartresponse.sourceAddress);
                        return Ok(statusresponse);
                    default:
                        break;
                }

            //TO-DO your self
            //manage page navigation
            //store data into a database if you need them
            }

            return BadRequest();
        }
    }
}
