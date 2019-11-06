

import json
import requests
from . import (config, exception)

class Weather(object):

    _session = None
    __DEFAULT_BASE_URL = 'https://api.openweather.net/forecast/{0}/{1},{2}?units=ca'
    __DEFAULT_TIMEOUT = 10

    def __init__(self, base_url = __DEFAULT_BASE_URL, request_timeout = __DEFAULT_TIMEOUT):
        self.base_url = base_url
        self.config = config.load()
        self.request_timeout = request_timeout

    
    def __request(self):
        response_object = self.session.get(self.base_url.format(self.config.get("darksky", "api_key"),
                                                                self.config.get("darksky", "latitude"),
                                                                self.config.get("darksky", "longitude")
                                                                ), timeout = self.request_timeout)

        try:
            response = json.loads(response_object.text)
        except Exception as e:
            raise exception.WeatherProviderError("{}".format(e))
        return response

    def data(self):
        response = self.__request()
        return response
