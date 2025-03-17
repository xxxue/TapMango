using System;
using System.Runtime.CompilerServices;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace TapMangoMicroservice.Controller
{
    public class SendMessage
    {
        //Set two value to test 
        public static int MAX_NUMBER_PER_SECOND_PER_PHONE = 10;
        public static int MAX_NUMBER_PER_SECOND_PER_ACCOUNT = 30;


        /// <summary>
        /// This method gets inputs from front-end and save into database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<ActionResult> SendMessage(Request request)
        {
            var time = DateTime.UtcNow;

            _DbContext.MessageDetails.Add(new MessageDetails()
            {
                BusinessID = request.BusinessID,
                PhoneNumber = request.PhoneNumber,
                MessageBody = request.MessageBody,
                SentTime = request.SentTime,
                AccountID = request.AcctionID

            });

            _dbContext.SubmitChanges();
            return (new "Send Message successfully!");
        }



        [HttpGet]
        public ActionResult CanMessageSend (int businessID, int accountID)
        {
            var time = DateTime.UtcNow;
            int businessCount = GetMessageStates(businessID);
            int accountCount = GetAccountStates(accountID);

            if (businessCount > MAX_NUMBER_PER_SECOND_PER_PHONE && accountCount > MAX_NUMBER_PER_SECOND_PER_ACCOUNT)
                return (new { result = "OK"}); 
            else
                return (new { result = "BAD REQUEST" }); 
            
        }


        /// <summary>
        /// To Determine count of each business 
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        [HttpGet]
        public int GetMessageStates (int businessID)
        {
            var now = DateTime.UtcNow;
            var count = _DbContext.MesssageDetails.Where(x => businessID == x.BusinessId && x.SentTime == now.AddSeconds(-1)).Count();
            return count;
        }

        /// <summary>
        /// To Determine count of each account
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [HttpGet]
        public int GetAccountStates (int accountID)
        {
            var now = DateTime.UtcNow;
            var count = _DbContext.MessageDetails.Where(x => accountID == x.AccountID).GroupBy(x => x.AccountID).Count();
            return count;
        }

    }


}
