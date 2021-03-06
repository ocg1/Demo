﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auctus.Service;
using Microsoft.Extensions.Logging;
using Auctus.Web.Model.Home;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.SignalR.Infrastructure;
using Auctus.Web.Hubs;
using Auctus.DomainObjects.Contracts;

namespace Web.Controllers
{
    public class HomeController : HubBaseController
    {
        public HomeController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, IConnectionManager connectionManager) : base(loggerFactory, cache, serviceProvider, connectionManager) { }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Register")]
        public IActionResult StartDemo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(Wizard model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentException("Invalid data.");
                if (!IsValidRecaptcha(model.Captcha))
                    throw new InvalidOperationException("Invalid captcha.");

                var pensionFundContract = PensionFundsServices.CreateCompleteEntry(model.Fund, model.Company, model.Employee);
                return Json(pensionFundContract);
            }
            catch(Exception e)
            {
                Response.StatusCode = 400;
                if (e is ArgumentException)
                    return Json(e.Message);
                return Json("We’re sorry, we had an unexpected error! Please try again in a minute.");
            }
        }

        [HttpPost]
        public IActionResult CheckContractCreationTransaction(String transactionHash)
        {
            if (!string.IsNullOrEmpty(ConnectionId))
            {
                Task.Factory.StartNew(() =>
                {
                    var hubContext = HubConnectionManager.GetHubContext<AuctusDemoHub>();
                    try
                    {
                        var pensionFundContract = PensionFundsServices.CheckContractCreationTransaction(transactionHash);
                        if (pensionFundContract.BlockNumber.HasValue)
                            hubContext.Clients.Client(ConnectionId).deployCompleted(Json(
                                new
                                {
                                    Address = pensionFundContract.Address,
                                    BlockNumber = pensionFundContract.BlockNumber,
                                    TransactionHash = pensionFundContract.TransactionHash
                                }));
                        else
                            hubContext.Clients.Client(ConnectionId).deployUncompleted(pensionFundContract.TransactionHash);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(new EventId(1), ex, string.Format("Erro on CheckContractCreationTransaction {0}.", transactionHash));
                        hubContext.Clients.Client(ConnectionId).deployError();
                    }
                });
                return Json(new { success = true });
            }
            else
                return Json(new { success = false });
        }
    }
}
