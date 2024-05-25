//
//  Sensor.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 25.05.2024.
//

import Foundation

struct Sensor:Codable{
    public var id:Int
    public var notes:String?
    public var frequency:Int
    public var aviaryId:Int?
    
    public static func getSensor(aviaryId:Int, completion: @escaping (Result<Sensor?, Error>)-> Void){
        let url = "api/Aviary/sensor/\(aviaryId)"
        
        print("Getting aviary sensor")
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
                    if let decodedData = HttpClient.deserializeObject(stringData, type: Sensor.self) {
                        print(decodedData)
                        completion(Result.success(decodedData))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    public func updateSensor(completion: @escaping (Bool)-> Void){
        let url = "api/Aviary/update/sensor"
        
        var httpClient = HttpClient.createRequest(url: url, method: .PUT)
        if(httpClient == nil){
            completion(false)
            return
        }
        if let jsonData = HttpClient.serializeObject(self){
            httpClient?.httpBody = jsonData
        }
        
        let task = URLSession.shared.dataTask(with: httpClient!){ (data, response, error) in
            if let error = error {
                print("Error: \(error)")
                return
            }
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Invalid response")
                return
            }
            print(httpResponse.statusCode)
            if(httpResponse.statusCode == 200){
                print("Succesful sensor update")
                completion(true)
                return
            }
        }
        task.resume()
    }
    
    
}
