﻿using InterviewPrep.OdeToFoodCore.DataAccess;
using InterviewPrep.OdeToFoodCore.Entities;
using InterviewPrep.OdeToFoodCore.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;

namespace InterviewPrep.OdeToFoodCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepository<Restaurant> _restaurantRepository;

        public HomeController(IRepository<Restaurant> restaurantRepository)
        {
            if (restaurantRepository == null)
                throw new ArgumentNullException(nameof(restaurantRepository));
            _restaurantRepository = restaurantRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new HomePageViewModel();
            model.Restaurants = _restaurantRepository.GetAll();
            return View(model);
            //return new ObjectResult(restaurant);
            //return Content(_greeter.GetGreeting());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RestaurantEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View();
            var restaurant = new Restaurant { Name = model.Name, Cuisine = model.Cuisine };
            _restaurantRepository.Add(restaurant);
            _restaurantRepository.Commit();
            return RedirectToAction("Details", new { id = restaurant.Id });
        }

        public IActionResult Details(int id)
        {
            var restaurant = _restaurantRepository.Get(id);
            if (restaurant == null)
                return RedirectToAction("Index");
            return View(restaurant);
        }

        public IActionResult Edit(int id)
        {
            var model = _restaurantRepository.Get(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, RestaurantEditViewModel inputModel)
        {
            using (_restaurantRepository)
            {
                var restaurant = _restaurantRepository.Get(id);
                if (restaurant == null || !ModelState.IsValid)
                    return View(restaurant);
                restaurant.Name = inputModel.Name;
                restaurant.Cuisine = inputModel.Cuisine;
                //_restaurantRepository.Update(restaurant);
                _restaurantRepository.Commit();
                return RedirectToAction("Details", new { id = restaurant.Id });
            }
        }

        public IEnumerable<Restaurant> GetAll()
        {
            var restaurants = _restaurantRepository.GetAll();
            return restaurants;
        }
    }
}