using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BankAPI.Models;

namespace BankAPI.Controllers
{
    public class TransactionController : ApiController
    {
        OnlineBankEntities1 dbContext = new OnlineBankEntities1();
        public int getCustomerId(long accountNo)
        {
            accountDetail account = dbContext.accountDetails.Where(val => val.accountNumber == accountNo).Single<accountDetail>();
            int userId = int.Parse(account.userId.ToString());
            return userId;
        }
        [HttpPost]
        public HttpResponseMessage insertTransaction(long SourceAccount, long destinationAccount, int amt, string type, string comment)
        {
            int userId = getCustomerId(SourceAccount);
            DateTime date = DateTime.Now;
            transactionDetail transactionInsert = new transactionDetail
            {
                sourceAccountNumber = SourceAccount,
                destinationAccountNumber = destinationAccount,
                transactionDate = date,
                transactionAmount = amt,
                transactionType = type,
                comments = comment,
                transactionAuthorizedBy = userId


            };
            dbContext.transactionDetails.Add(transactionInsert);
            dbContext.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);

        }

    }
}
