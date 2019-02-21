# RESTful-Weather-Provider
This C# weather application is designed so that users can input a city that they would like to check weather details on. The weather data for this program is retrieved by utilizing a free RESTful API service found on openweathermap.org As a result of the query sent to the API with a specific city name and an app_id, a JSON file is returned containing the weather details on the chosen city. To make sense of the data, de-serialization is made to turn the contents of the JSON file to meaningful C# objects. After deserializing the JSON file, useful weather information is gathered by calling the states in of RootObject. Then, the weather details get printed via using the console.



#Compiling the program
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /t:exe /out:WeatherApp.exe WeatherApp.cs /r:System.Net.Http.dll /r:Newtonsoft.Json.dll

Then double click the generated new WeatherApp.exe to enter a city.
