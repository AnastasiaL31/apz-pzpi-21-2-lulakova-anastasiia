//
//  Staff.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 24.05.2024.
//

import Foundation


struct Staff: Codable, Identifiable{
    public var id:Int
    public var name:String
    public var phone:String
    public var dob:String
    public var position:String
    public var hasRole:Bool = false
    public var acceptanceDate:String
    public var dismissialDate:String?
    var identityUserId:String
    
    public var DOB:Date?{
        return DateConverter.fromSwiftDateStringToDate(from: dob)
    }
    
    public var AcceptanceDate:Date?{
        return DateConverter.fromSwiftDateStringToDate(from: self.acceptanceDate)
    }
    
    public var DissmisialDate:Date?{
        if let dissmisialDate = dismissialDate{
            return DateConverter.fromSwiftDateStringToDate(from: dissmisialDate)
        }else{
            return nil
        }
    }
    
    public static func getAllStaff(completion: @escaping (Result<Array<Staff>, Error>) -> Void){
        let url = "api/Staff/all"
        
        print("Getting all staff")
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
                    if let decodedData = HttpClient.deserializeObject(stringData, type: Array<Staff>.self) {
                        
                        print(decodedData)
                        completion(Result.success(decodedData))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    
    public func getStaffDetails(completion: @escaping (Result<StaffDetails, Error>) -> Void){
        let url = "api/Staff/all/\(self.id)"
        
        print("Getting staff details")
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
                    if let decodedData = HttpClient.deserializeObject(stringData, type: StaffDetails.self) {
                        print(decodedData)
                        completion(Result.success(decodedData))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    
    public func updateStaff(role:String, completion: @escaping (Bool) -> Void){
        let url = "api/Staff/update"
        
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
                print("Succesful staff update")
                completion(true)
                return
            }
            completion(false)
        }
        task.resume()
    }
    
    public func updateStaffRole(role:String,completion: @escaping (Bool) -> Void){
        let url = "api/Staff/addRole?roleName=\(role)&staffId=\(self.id)"
        
        var httpClient = HttpClient.createRequest(url: url, method: .PUT)
        if(httpClient == nil){
            completion(false)
            return
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
                print("Succesful role update")
                completion(true)
                return
            }
        }
        task.resume()
    }
    
    
}
