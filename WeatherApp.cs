/**
 *  @Auther: Fahad Alshehri
 *  
 *  
 *  Discription:
 *  RESTful APIs from openweathermap.org are used to provide wheather data.
 *  APIs calls are made using a GET HTTP request and the response is returned as a JSON file. 
 *  
 *  Deserialization on the the JSON file is made to turn the data provided by the API into objects to use them for programming purposes 
 * */

using System;
using System.Net.Http;
using Newtonsoft.Json;



namespace CloudComputing_Program2 {

    class WeatherApp {


        /**********************************************************************
        *  An entry point to the program
        * ****************************************/
        static void Main(string[] args) {

            Console.WriteLine("-------Welcome to Fahad's wheather app! --------");

            // Calling the RESTful API
            requestWeather();

        }

        /**********************************************************************
        *  Makes a request to weather API based on the city provided by the user
        * ****************************************/
        static private void requestWeather() {

            // check for validating user input
            string check = "";
            //Easy access to my app ID
            string myAppID = "97b0dfe685ec5a2099de6e00fd9b876c";

            do {
                
                Console.Write("Please enter a city: ");
                string chosenCity = Console.ReadLine();

                using (var client = new HttpClient()) {

                    client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/");

                    String apiCall = "weather?q=" + chosenCity + "&APPID=" + myAppID;

                    HttpResponseMessage response = client.GetAsync(apiCall).Result;

                    //ensures that the http connection is succesful.
                    if (response.IsSuccessStatusCode == false) {
                        Console.WriteLine();
                        Console.Error.Write(" City cannot be found. Please try again: ");
                        requestWeather();
                    }
                    response.EnsureSuccessStatusCode();

                    //provides the HTTP response result
                    string result = response.Content.ReadAsStringAsync().Result;

                    Rootobject weatherDetails = deserialize(result);

                    printWeatherDetails(weatherDetails, chosenCity);

                    Console.Write("Would you like to check out the weather on another city? enter: (y or n) ");
                    check = Console.ReadLine().Trim();
                    //Incase the user wants to exit the program
                    if (check.Equals("n")) {
                        Console.WriteLine();
                        Console.WriteLine("---------- Thank you for using the app! -------- ");
                        System.Environment.Exit(0);
                    }

                }

                // keep doing this while the answer is "y"
            } while (check.Equals("y") == true);
        }

        /******************************************************
        *  Deserializes the Json data provided by the API
        * ****************************************/
        static private Rootobject deserialize(String result) {
            //Disrializing json into objects
            Rootobject weatherDetails = JsonConvert.DeserializeObject<Rootobject>(result);
            return weatherDetails;
        }



        /******************************************************
         *  prints the weather details provided from the API
         * ****************************************/
        static private void printWeatherDetails(Rootobject weatherDetails, String chosenCity) {

            Console.WriteLine();
            Console.WriteLine("--- Getting the weather details for (" + chosenCity + "): ");
            Console.WriteLine();

            string country = weatherDetails.sys.country;
            Console.WriteLine("Fun fact! " + chosenCity.ToUpper() + " is a city in " + country);
            Console.WriteLine();

            string currWeather = "";
            foreach (Weather item in weatherDetails.weather)
                currWeather = currWeather + item.main + " ";
            Console.WriteLine("- Current Conditions: " + currWeather);

            float kelvinTemp = weatherDetails.main.temp;
            int temp = (int)((kelvinTemp - 273.15) * 1.8 + 32);
            Console.WriteLine("- Current Tempature: " + temp + "°F");

            float kelvinMinTemp = weatherDetails.main.temp_min;
            int minTemp = (int)((kelvinMinTemp - 273.15) * 1.8 + 32);
            Console.WriteLine("- Current Minimum Tempature: " + minTemp + "°F");


            float kelvinMaxTemp = weatherDetails.main.temp_max;
            int maxTemp = (int)((kelvinMaxTemp - 273.15) * 1.8 + 32);
            Console.WriteLine("- Current Maximum Tempature: " + maxTemp + "°F");

            double windSpeed = (int)weatherDetails.wind.speed;
            Console.WriteLine("- Current Wind speed " + windSpeed + "mph");


            int humidity = weatherDetails.main.humidity;
            Console.WriteLine("- Current Humidity " + humidity + "%");

            int pressure = weatherDetails.main.pressure;
            Console.WriteLine("- Current Pressure " + pressure + "hPa");

            DateTime currTime = getCurrentTime(weatherDetails.dt);
            Console.WriteLine("- Current UTC Date and Time: " + currTime);

            Console.WriteLine();
            Console.WriteLine("--------------------------");
            Console.WriteLine();
        }

        /******************************************************
        *  Converts unix time to UTF time
         * ****************************************/
        static private DateTime getCurrentTime(double unixTime) {
            DateTime dateAndTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateAndTime.AddSeconds(unixTime);
        }

        public class Rootobject {
            public Coord coord { get; set; }
            public Weather[] weather { get; set; }
            public string _base { get; set; }
            public MainApp main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }

        public class Coord {
            public float lon { get; set; }
            public float lat { get; set; }
        }

        public class MainApp {
            public float temp { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public float temp_min { get; set; }
            public float temp_max { get; set; }
        }

        public class Wind {
            public float speed { get; set; }
            public float deg { get; set; }
        }

        public class Clouds {
            public int all { get; set; }
        }

        public class Sys {
            public int type { get; set; }
            public int id { get; set; }
            public float message { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class Weather {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

    }

}


