namespace ShowMeTheWeather.Utils
{
    using System.Collections.Generic;
    using System.Xml;
    /// <summary>
    /// Parses Xml data that have been read from web site to display it to user
    /// </summary>
    public static class XmlWeatherReader
    {
        /// <summary>
        ///  Parses data about current weather conditions
        /// </summary>
        /// <param name="xmlConditions">document with xml data</param>
        /// <param name="titleString">page title stirng</param>
        /// <returns>current(single) weather conditions</returns>
        public static Model.WeatherConditions BuildCurrentWeatherConditionsFromXmlDocument(XmlDocument xmlConditions, out string titleString)
        {
            Model.WeatherConditions weatherConditions = new Model.WeatherConditions();
            XmlNode node = xmlConditions.DocumentElement.SelectNodes("/current")[0];

            titleString = string.Format("{0} {1}", node.SelectSingleNode("city").SelectSingleNode("country").InnerText, node.SelectSingleNode("city").Attributes["name"]?.Value);
            weatherConditions.Temperature = string.Format("Temperature: {0} degrees", node.SelectSingleNode("temperature").Attributes["value"]?.Value);
            weatherConditions.Humidity = string.Format("Humidity: {0} %", node.SelectSingleNode("humidity").Attributes["value"]?.Value);
            weatherConditions.Pressure = string.Format("Pressure: {0} hPa", node.SelectSingleNode("pressure").Attributes["value"]?.Value);
            weatherConditions.Visibility = string.Format("Visibility: {0}", node.SelectSingleNode("visibility").Attributes["value"]?.Value);
            weatherConditions.WindDescription = string.Format("Wind: {0} {1} km/h", node.SelectSingleNode("wind").SelectSingleNode("direction").Attributes["name"]?.Value
                , node.SelectSingleNode("wind").SelectSingleNode("speed").Attributes["value"].Value);
            return weatherConditions;
        }
        /// <summary>
        /// Parses data about forecast
        /// </summary>
        /// <param name="xmlDocument">document with xml data</param>
        /// <param name="titleString">page title string</param>
        /// <returns>dictionary with conditions</returns>
        public static Dictionary<string, Model.WeatherConditions> BuildForecastsFromXmlDocument(XmlDocument xmlDocument, out string titleString)
        {
            Dictionary<string, Model.WeatherConditions> forecasts = new Dictionary<string, Model.WeatherConditions>();
            XmlNode mainNode = xmlDocument.DocumentElement.SelectNodes("/weatherdata")[0];
            titleString = string.Format("{0} {1}", mainNode.SelectSingleNode("location").SelectSingleNode("country")?.InnerText
                , mainNode.SelectSingleNode("location").SelectSingleNode("name")?.InnerText);
            foreach (XmlNode node in xmlDocument.DocumentElement.SelectNodes("/weatherdata/forecast/time"))
            {
                string key = string.Format("FROM {0} TO {1}", node.Attributes["from"].Value, node.Attributes["to"].Value);
                Model.WeatherConditions currentWeatherCondition = new Model.WeatherConditions();
                currentWeatherCondition.Temperature = string.Format("Temperature: {0} degress", node.SelectSingleNode("temperature").Attributes["value"]?.Value);
                currentWeatherCondition.Humidity = string.Format("Humidity: {0} %", node.SelectSingleNode("humidity").Attributes["value"]?.Value);
                currentWeatherCondition.Pressure = string.Format("Pressure: {0} hPa", node.SelectSingleNode("pressure").Attributes["value"]?.Value);
                currentWeatherCondition.WindDescription = string.Format("Wind: {0} {1} km/h", node.SelectSingleNode("windDirection").Attributes["name"]?.Value
                    , node.SelectSingleNode("windSpeed").Attributes["mps"]?.Value);

                forecasts.Add(key, currentWeatherCondition);
            }
            return forecasts;
        }
    }
}
