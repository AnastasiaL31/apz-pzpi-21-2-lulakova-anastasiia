//
//  Aviary.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 23.05.2024.
//

import Foundation

struct Aviary: Codable{
    public var id:Int
    public var description:String?
    public var animalId:Int?
    public var aviaryConditionId:Int?
    public var aviaryCondition:AviaryCondition?
    
    
    
    public static func getAnimalAviary(animalId:Int, completion: @escaping (Result<Aviary, Error>) -> Void){
        let url = "api/Aviary/aviary/\(animalId)"
        
        print("Getting aviary")
        var httpClient = HttpClient.createRequest(url: url, method: .GET)
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
                    if let decodedData = HttpClient.deserializeObject(stringData, type: Aviary.self) {
                        print(decodedData)
                        completion(Result.success(decodedData))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    public func updateAviary(completion: @escaping (Bool) -> Void){
        let url = "api/Aviary/update"
        
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
                print("Succesful aviary update")
                completion(true)
                return
            }
        }
        task.resume()
    }
    
    public func addAviaryCondition(completion: @escaping (Result<Int, Error>) -> Void){
        let url = "add/condition?aviaryId=\(self.id)"
        
        var httpClient = HttpClient.createRequest(url: url, method: .POST)
        if(httpClient == nil){
            completion(Result.failure(RequestError.WrongRequest))
            return
        }
        if let jsonData = HttpClient.serializeObject(self.aviaryCondition){
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
                if let data = data {
                    if let stringData = String(data: data, encoding: .utf8) {
                        if let newId = Int(stringData){
                            completion(Result.success(newId))
                        }
                        completion(Result.failure(RequestError.WrongData))
                        return
                    }
                }
                
            }
        }
        task.resume()
    }
    
}

struct AviaryCondition: Codable{
    public var id:Int
    public var minWater:Float = 0
    public var food:Float  = 0
    public var minTemperature:Float = 0
    public var maxTemperature:Float = 0
    public var minHumidity:Float = 0
    public var maxHumidity:Float = 0
    
}
