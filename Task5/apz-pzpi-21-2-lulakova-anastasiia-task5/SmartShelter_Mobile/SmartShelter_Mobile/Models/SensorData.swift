//
//  SensorData.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 25.05.2024.
//

import Foundation


struct SensorData: Codable, Identifiable{
    public var id:Int
    public var water:Float
    public var food:Float
    public var temperature:Float
    public var humidity:Float
    public var ihs:Float
    public var date:String
    public var forecast:Float
    public var sensorId:Int
    
    
    public static func getSensorData(sensorId:Int,completion:  @escaping (Result<Array<SensorData>, Error>)-> Void){
        let url = "api/Aviary/sensordata/\(sensorId)"
        
        print("Getting sensor data\n")
        let httpClient = HttpClient.createRequest(url: url, method: .GET)
        if(httpClient == nil){
            completion(Result.failure(RequestError.WrongRequest))
            return
        }
        
        let task = URLSession.shared.dataTask(with: httpClient!){ (data, response, error) in
            
            if(!HttpClient.checkResponseAndError(response: response, error: error)){
                completion(Result.failure(error ?? RequestError.WrongRequest))
                return
            }
            if let data = data {
                if let stringData = String(data: data, encoding: .utf8) {
                    print(stringData)
                    if let decodedData = HttpClient.deserializeObject(stringData, type: Array<SensorData>.self) {
                        print(decodedData)
                        completion(Result.success(decodedData))
                        return
                    }
                }
            }
        }
        task.resume()
    }
}
