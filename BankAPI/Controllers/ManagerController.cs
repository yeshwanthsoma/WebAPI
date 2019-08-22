using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BankAPI.Models;

namespace BankAPI.Controllers
{
    public class ManagerController : ApiController
    {
         OnlineBankEntities1 dbContext = new OnlineBankEntities1();
        [HttpGet]
        [Route("api/MAnager/getAllAccountsOfBranchCustomer")]
        public HttpResponseMessage getAllAccountsOfBranchCustomer(int userId,string loginId)
        {
            userDetail manager=dbContext.userDetails.Where(x=>x.emailId==loginId).Single<userDetail>();
            List<accountDetail> accounts=new List<accountDetail>();
            try
            {
               
                userDetail allcustomer = dbContext.userDetails.Single(x => x.userId == userId && x.branchId == manager.branchId);
                accounts = (dbContext.accountDetails.Where(x => x.userId == allcustomer.userId)).ToList();
            }
            catch (Exception exe)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This is not your branch customer");
            }
            return Request.CreateResponse(HttpStatusCode.OK, accounts);
        } 
        
    }
}
