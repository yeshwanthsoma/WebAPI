using BankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BankAPI.Controllers
{
    public class LoginController : ApiController
    {
        OnlineBankEntities1 dbContext = new OnlineBankEntities1();

        public string Decrypt(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }
       
        
        [HttpGet]
        public HttpResponseMessage checkCredentials(string loginId, string password)
        {
                string role;
                try
                {
                    loginDetail dbObj = (loginDetail)dbContext.loginDetails.SingleOrDefault(x => x.loginId == loginId);
                    if (dbObj == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound,"invalid username");
                    }
                    else
                    {
                        string Dpassword = Decrypt(dbObj.loginPassword);
                        if (password == Dpassword)
                        {
                            role = dbObj.userRole.ToString();
                            return Request.CreateResponse(HttpStatusCode.OK, role);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound,"invalid Password");
                        }
                    }
                    
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,e);
                }

        }

    }
}
