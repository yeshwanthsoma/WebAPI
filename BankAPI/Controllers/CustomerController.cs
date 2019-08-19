using BankAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankAPI.Controllers
{
    public class CustomerController : ApiController
    {
        OnlineBankEntities1 dbContext = new OnlineBankEntities1();

        [HttpGet]
        public HttpResponseMessage getAccountsOfUser(string loginId)
        {
            userDetail customer = dbContext.userDetails.Where(x => x.emailId == loginId).Single<userDetail>();
            List<accountDetail> accounts = dbContext.accountDetails.Where(y => y.userId == customer.userId).ToList();
            List<accountDetailViewModel> acc3 = new List<accountDetailViewModel>();


            int i = 0;
            foreach (accountDetail xyz in accounts)
            {
                accountDetailViewModel acc2 = new accountDetailViewModel();
                acc2.accountBalance = xyz.accountBalance;
                acc2.accountNumber = xyz.accountNumber;
                acc2.accountStatus = xyz.accountStatus;
                acc2.accountType = xyz.accountType;
                acc2.closingDate = xyz.closingDate;
                acc2.createdBy = xyz.createdBy;
                acc2.createdDate = xyz.createdDate;
                acc2.editedDate = xyz.editedDate;
                acc2.type = xyz.type;
                acc2.updatedBy = xyz.updatedBy;
                acc2.userId = xyz.userId;
                acc3.Add(acc2);
            }

            return Request.CreateResponse(HttpStatusCode.OK, accounts);            
        }
        [HttpGet]
        public HttpResponseMessage getSpecificAccountOfUser(long accountNo)
        {
            accountDetail account = dbContext.accountDetails.Where(x => x.accountNumber == accountNo).Single<accountDetail>();
            accountDetailViewModel acc2 = new accountDetailViewModel();
            
            
            //acc2.accountBalance = account.accountBalance;
            //acc2.accountNumber = xyz.accountNumber;
            //acc2.accountStatus = xyz.accountStatus;
            //acc2.accountType = xyz.accountType;
            //acc2.closingDate = xyz.closingDate;
            //acc2.createdBy = xyz.createdBy;
            //acc2.createdDate = xyz.createdDate;
            //acc2.editedDate = xyz.editedDate;
            //acc2.type = xyz.type;
            //acc2.updatedBy = xyz.updatedBy;
            //acc2.userId = xyz.userId;
            return Request.CreateResponse(HttpStatusCode.OK, account);   
            

        }
    }
}
