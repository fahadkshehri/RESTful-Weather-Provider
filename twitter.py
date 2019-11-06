

import tweepy
from . import (config, exception, weather)

TWEET_FMT = """
Today's #weather #forecast for #Luxembourg:

Currently {condition} and {temp}°C with {wind}km/h winds and {humidity}% humidity.
Forecast: low {forecast_min}°C, high {forecast_max}°C"""

class Bot(object):
    def __init__(self):
        self.config = config.load()

    def __auth(self):
        auth = tweepy.OAuthHandler(self.config.get("twitter", "consumer_key"),
                                   self.config.get("twitter", "consumer_secret"))
        auth.set_access_token(self.config.get("twitter", "access_token"),
                              self.config.get("twitter", "access_secret"))
        return auth

    def __fmt_tweet(self):
        data = weather.Weather().data()
        fmt_tweet = TWEET_FMT.format(condition = data["currently"]["summary"].lower(),
                                     temp = int(data["currently"]["temperature"]),
                                     wind = int(data["currently"]["windSpeed"]),
                                     humidity = int(data["currently"]["humidity"] * 100),
                                     forecast_min = int(data["daily"]["data"][0]["temperatureMin"]),
                                     forecast_max = int(data["daily"]["data"][0]["temperatureMax"]))
        return fmt_tweet

    def post(self):
        try:
            api = tweepy.API(self.__auth())
            api.update_status(self.__fmt_tweet())
        except Exception as e:
            raise exception.TwitterProviderError("{}".format(e))
