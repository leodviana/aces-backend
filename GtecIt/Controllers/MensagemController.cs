using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using System.Collections.Generic;
using System.IO;
using GtecIt.Util;
using GtecIt.ViewModels;
using GtecIt.Domain.Entities;
using GtecIt.Infra.Data.Core;
using Microsoft.Ajax.Utilities;

namespace GtecIt.Controllers
{
    public class MensagemController : Controller
    {
        private readonly IUnitOfWork _uoW;

        public MensagemController(IUnitOfWork uoW)
        {
            _uoW = uoW;

        }

        public ActionResult Index(RelGeralViewlModel model)
        {
           
            return View(model);

        }

       
    }
}
