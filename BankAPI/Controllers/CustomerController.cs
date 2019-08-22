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

        public string Encrypt(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }


        [HttpGet]
        [Route("api/Customer/getAccountsOfUser")]
        public HttpResponseMessage getAccountsOfUser(string loginId)
        {
            userDetail customer = dbContext.userDetails.Where(x => x.emailId == loginId).Single<userDetail>();
            List<accountDetail> accounts = dbContext.accountDetails.Where(y => y.userId == customer.userId).ToList();
            
            return Request.CreateResponse(HttpStatusCode.OK, accounts);            
        }

        [HttpGet]
        [Route("api/Customer/getSpecificAccountOfUser")]
        public HttpResponseMessage getSpecificAccountOfUser(long accountNo)
        {
            accountDetail account = dbContext.accountDetails.Where(x => x.accountNumber == accountNo).Single<accountDetail>();
            return Request.CreateResponse(HttpStatusCode.OK, account);   
        }

        [HttpGet]
        [Route("api/Customer/getAccountTypeOfGivenAmount")]
        public HttpResponseMessage getAccountTypeOfGivenAmount(long amount)
        {
            accountTypeDetail accountType = dbContext.accountTypeDetails.Where(x => x.lowerBound <= amount && x.upperBound >= amount).Single<accountTypeDetail>();
            return Request.CreateResponse(HttpStatusCode.OK, accountType);   
        }

        [HttpGet]
        [Route("api/Customer/checkDestinationAccount")]
        public HttpResponseMessage checkDestinationAccount(long accountNo)
        {
            try
            {
                accountDetail acc = dbContext.accountDetails.Where(val => val.accountNumber == accountNo).Single<accountDetail>();
            }
            catch (Exception exe)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, false);
            }
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [HttpGet]
        [Route("api/Customer/getAmount")]
        public HttpResponseMessage getAmount(long accountNo)
        {
            try
            {
                accountDetail account = dbContext.accountDetails.Where(val => val.accountNumber == accountNo).Single<accountDetail>();
                int amt = (int)(account.accountBalance);
                return Request.CreateResponse(HttpStatusCode.OK, amt);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, -100);
            }
        }

        [HttpPost]
        [Route("api/Customer/FundTransfer")]
        public HttpResponseMessage FundTransfer(int amount, long destinationAccountNo,long SourceAccountNo)
        {
            try
            {
                accountDetail account = dbContext.accountDetails.Where(val => val.accountNumber == destinationAccountNo).Single<accountDetail>();
                account.accountBalance += amount;
                accountTypeDetail accountType = dbContext.accountTypeDetails.Where(x => x.lowerBound <= account.accountBalance && x.upperBound >= account.accountBalance).Single<accountTypeDetail>();
                account.accountType = accountType.accountType;
                accountDetail account1 = dbContext.accountDetails.Where(val => val.accountNumber == SourceAccountNo).Single<accountDetail>();
                account1.accountBalance -= amount;
                 accountType = dbContext.accountTypeDetails.Where(x => x.lowerBound <= account1.accountBalance && x.upperBound >= account1.accountBalance).Single<accountTypeDetail>();
                account1.accountType = accountType.accountType;
                dbContext.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception exe)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }
        }

        [HttpGet]
        [Route("api/Customer/GenerateMiniStatement")]
        public HttpResponseMessage GenerateMiniStatement(long accountNo)
        {
            List<transactionDetail> transactions = (List<transactionDetail>)(dbContext.transactionDetails.Where(x => x.sourceAccountNumber == accountNo || x.destinationAccountNumber == accountNo).OrderByDescending(x => x.transactionId).ToList());
            return Request.CreateResponse(HttpStatusCode.OK,transactions);
        }
        
        [HttpGet]
        [Route("api/Customer/customstatement")]
        public HttpResponseMessage customstatement(long acc, DateTime start, DateTime end)
        {
            List<transactionDetail> transactions = dbContext.transactionDetails.Where(val => (val.destinationAccountNumber == acc || val.sourceAccountNumber == acc) && (val.transactionDate > start) && (val.transactionDate < end)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK,transactions);
        }
        [HttpPost]
        [Route("api/Customer/changePassword")]
        public HttpResponseMessage changePassword(string oldPassword, string newPassword1, string newPassword2, string loginId)
        {
            try
            {
                loginDetail login = dbContext.loginDetails.Where(val => val.loginId == loginId).Single<loginDetail>();
                if (oldPassword == Decrypt(login.loginPassword))
                {
                    if (newPassword1 == newPassword2)
                    {
                        login.loginPassword = Encrypt(newPassword1);
                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "password Changed");
                     }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable,"Password Mismatch");
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Please enter correct old password");                  
                }
            }
            catch (Exception exe)
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "User not found");
            }
        }
    [HttpGet]
    [Route("api/Customer/BalanceEnquiry")]
        public HttpResponseMessage BalanceEnquiry(long accountNo)
        {
            try
            {
                accountDetail account = (dbContext.accountDetails.Single(x => x.accountNumber == accountNo));
                return Request.CreateResponse(HttpStatusCode.OK, account);

            }
            catch (Exception exe)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Account not found");
            }
            

        }
    
    
    }
}
