using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Facebook.Scrumptious.Windows8.ViewModel
{
    public class Friend
    {
        public string id { get; set; }
        public string Name { get; set; }
        public Uri PictureUri { get; set; }
    }

    public class Location
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Uri PictureUri { get; set; }
    }

    public class Meal
    {
        public string Name { get; set; }
        public string MealUri { get; set; }
    }

    public class FacebookData
    {
        private static ObservableCollection<Friend> friends = new  ObservableCollection<Friend>() ;
        public static ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
        }

        private static ObservableCollection<Friend> selectedFriends = new ObservableCollection<Friend>();
        public static ObservableCollection<Friend> SelectedFriends
        {
            get
            {
                return selectedFriends;
            }
        }

        private static Friend me = new Friend();
        public static Friend Me
        {
            get
            {
                return me;
            }
        }

        private static ObservableCollection<Location> locations = new ObservableCollection<Location>();
        public static ObservableCollection<Location> Locations
        {
            get
            {
                return locations;
            }
        }

        private static bool isRestaurantSelected = false;
        public static bool IsRestaurantSelected
        {
            get
            {
                return isRestaurantSelected;
            }
            set
            {
                isRestaurantSelected = value;
            }
        }

        public static Location SelectedRestaurant { get; set; }

        private static bool isLoadedMeals = false;
        private static ObservableCollection<Meal> meals = new ObservableCollection<Meal>();
        public static ObservableCollection<Meal> Meals
        {
            get
            {
                if (!isLoadedMeals)
                {
                    
                    meals.Add(new Meal { Name = "Pizza", MealUri = Constants.FBActionBaseUri + "pizza.html" });
                    meals.Add(new Meal { Name = "Cheeseburger", MealUri = Constants.FBActionBaseUri + "cheeseburger.html" });
                    meals.Add(new Meal { Name = "Hotdog", MealUri = Constants.FBActionBaseUri + "hotdog.html" });
                    meals.Add(new Meal { Name = "Italian", MealUri = Constants.FBActionBaseUri + "italian.html" });
                    meals.Add(new Meal { Name = "French", MealUri = Constants.FBActionBaseUri + "french.html" });
                    meals.Add(new Meal { Name = "Chinese", MealUri = Constants.FBActionBaseUri + "chinese.html" });
                    meals.Add(new Meal { Name = "Thai", MealUri = Constants.FBActionBaseUri + "thai.html" });
                    meals.Add(new Meal { Name = "Indian", MealUri = Constants.FBActionBaseUri + "indian.html" });
                    isLoadedMeals = true;
                }

                return meals;
            }
        }

        private static Meal selectedMeal = new Meal { Name = String.Empty, MealUri = String.Empty };
        public static Meal SelectedMeal
        {
            get
            {
                return selectedMeal;
            }

            set
            {
                selectedMeal = value;
            }
        }
    }
}
