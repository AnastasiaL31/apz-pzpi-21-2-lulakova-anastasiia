
#include <WiFi.h>
#include "DHTesp.h"
#include <HTTPClient.h>
#include <time.h>
#include <NTPClient.h>
#include <ArduinoJson.h>
#include <EEPROM.h>



#define EEPROM_SIZE 30

int loopIteration = 0;
String SSID = "Wokwi-GUEST";
String Password = "";
WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, "pool.ntp.org");
String UTCTime = "";
String serverAddress = "https://smartshelterapi.azurewebsites.net";
const int sensorId = 1;
int DHT_PIN = 15;
const int DHT_PIN_Hum = 18;
const int NTC_PIN = 39;
const int FOOD_PIN = 35;
const int WATER_PIN = 34;


float humidity = 0;
float temperature = 0;
float IHS = 0;
float forecast = 0;
const float BETA = 3950;
float water = 0;
float food = 0;

int n = 5;
int frequency = 3600000;
int counter = frequency/(60000 * n);
int isNTC = 0;
DHTesp dhtSensor;


void setup() {
  Serial.begin(9600);

  if(digitalRead(DHT_PIN_Hum) == HIGH){
    analogReadResolution(10);
    pinMode(NTC_PIN,INPUT);
    isNTC = 1;
    DHT_PIN = DHT_PIN_Hum;
    Serial.println("NTC is connected!");
  }
  dhtSensor.setup(DHT_PIN, DHTesp::DHT22);
  
  Serial.begin(115200);
  pinMode(WATER_PIN, INPUT);
  pinMode(FOOD_PIN, INPUT);
  Serial.print("Connecting to WiFi");
  WiFi.begin(SSID, Password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(100);
    Serial.print(".");
  }
  Serial.println(" Connected!");
  Serial.print("OK! IP=");
  Serial.println(WiFi.localIP());
  
  getDataFrequency();
  timeClient.begin();
  timeClient.setTimeOffset(0);
}

void loop(){
  GetAndPrintSensorData();
  if(loopIteration == counter){
    sendPostRequest();
    loopIteration = 0;
  }
  
  loopIteration +=1;
  Serial.print("Frequency: ");
  Serial.println(frequency);
  Serial.println("\n------------------------\n");
  delay(6000*n);
}

void getDataFrequency(){
    HTTPClient http;
    Serial.println("Getting frequency...");
    String GetUrl = "/Aviary/time/";
    http.begin(serverAddress + GetUrl + String(sensorId));
    int httpCode = http.GET();

    if (httpCode > 0) {
      
      if (httpCode == HTTP_CODE_OK) {
        String payload = http.getString();
        Serial.println(payload);
        frequency = payload.toInt(); 
      } else {
        Serial.println("HTTP request failed with error code: " + String(httpCode));
      }
    } else {
      Serial.println("Connection failed");
    }
    http.end();
}

void checkIHS(){
  Serial.println("Check heat index: ");
    if(IHS > 70){
      HTTPClient http;
      const String path = "/extreme?sensorId="+ String(sensorId) + "&ihs=" + String(IHS);
       http.begin(serverAddress + path);
       http.addHeader("accept", "*/*");
    int httpResponseCode = http.POST("");
    if (httpResponseCode > 0) {
      Serial.print("HTTP Response code: ");
      Serial.println(httpResponseCode);
      String response = http.getString();
      Serial.println(response);
    } else {
      Serial.print("Error code: ");
      Serial.println(httpResponseCode);
    }
    http.end();
    }
}

void sendPostRequest() {
  Serial.println("POST data to server...");
  HTTPClient http;
  UTCTime = getCurrentUTCTime();
  const String path = "/add/sensordata";
  http.begin(serverAddress + path);
  http.addHeader("Content-Type", "application/json-patch+json");
  String jsonData = R"({
    "water": )" + String(water) + R"(,
    "food": )" + String(food) + R"(,
    "temperature": )" + String(temperature) + R"(,
    "humidity": )" + String(humidity) + R"(,
    "ihs": )" + String(IHS) + R"(,
    "date": ")" + String(UTCTime) + R"(",
    "forecast": )" + String(forecast) + R"(,
    "sensorId": )" + String(sensorId) + R"(
  })";
   Serial.println(jsonData);
  
  int httpResponseCode = http.POST(jsonData);
  if (httpResponseCode > 0) {
    Serial.print("HTTP Response code: ");
    Serial.println(httpResponseCode);
    String response = http.getString();
    Serial.println(response);
  } else {
    Serial.print("Error code: ");
    Serial.println(httpResponseCode);
  }
  http.end();
}


float calculateIHS(){
  float res = (0.72 * temperature) + (0.5 * humidity) - (0.002 * temperature * humidity) + 40.6;
  return res;
}

float calculateWaterForecast(){
  if(temperature < 20){
    return 0.8;
  }else if(temperature >= 20 && temperature < 30){
    return 1;
  }else if(temperature >= 30){
    return 1.2;
  }
}


String getCurrentUTCTime(){

  timeClient.update();
  time_t rawTime = timeClient.getEpochTime();

  struct tm *timeinfo;
  timeinfo = localtime(&rawTime);

  char buffer[30];
  strftime(buffer, sizeof(buffer), "%Y-%m-%dT%H:%M:%S", timeinfo);
  String formattedDateTime(buffer);

  Serial.println("Date and Time: " + formattedDateTime);

  return formattedDateTime;
}

void GetAndPrintSensorData(){
 if(isNTC == 1){
  int analogValue = analogRead(NTC_PIN);
  temperature = 1 / (log(1 / (1023. / analogValue - 1)) / BETA + 1.0 / 298.15) - 273.15;
  Serial.println("NTC for temperature");
 }else{
  temperature = dhtSensor.getTemperature();
  Serial.println("DHT22 for temperature");
 }

  water = analogRead(WATER_PIN);
  Serial.print("Water: ");
  Serial.print(water);
  Serial.println(" ml");

  food = analogRead(FOOD_PIN);
  Serial.print("Food: ");
  Serial.print(food);
  Serial.println(" gr");

  humidity = dhtSensor.getHumidity();
  Serial.print("Humidity: ");
  Serial.print(humidity);
  Serial.println(" %\t");

  Serial.print("Temperature: ");
  Serial.print(temperature);
  Serial.println(" °C");
 
  IHS = calculateIHS();
  Serial.print("IHS: ");
  Serial.println(IHS);
  //checkIHS();

  forecast = calculateWaterForecast();
  Serial.print("Forecast water: ");
  Serial.println(forecast);

  if(loopIteration > 0){
    checkIHS();
  }

  if(temperature > 30 || temperature < 10){
    sendPostRequest();
  }

  if(humidity > 70 || humidity < 20){
    sendPostRequest();
  }
}
