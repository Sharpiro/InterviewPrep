﻿using System.ComponentModel.DataAnnotations;

namespace InterviewPrep.OdeToFoodRc2.ViewModels
{
    public class RegisterViewModel
    {
        [Required, MaxLength(256)]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}