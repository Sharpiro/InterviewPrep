﻿using Microsoft.AspNetCore.Mvc;

namespace InterviewPrep.OdeToFoodRc2.Controllers
{
    [Route("company/[controller]/[action]")]
    public class AboutController
    {
        //[Route("")]
        public string Phone()
        {
            return "a phone number";
        }

        //[Route("[action]")]
        public string Country()
        {
            return "Murica";
        }
    }
}