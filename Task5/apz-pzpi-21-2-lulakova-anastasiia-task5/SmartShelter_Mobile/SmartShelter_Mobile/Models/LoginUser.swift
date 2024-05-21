//
//  LoginUser.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 20.05.2024.
//

import Foundation

struct LoginUser : Encodable {
    
    public var Username:String
    public var Password:String
    
    
    //login
    public func GetUser(completion: @escaping (Result<Bool, Error>) -> Void) {
        let url = "api/Auth/Login"
        
        var httpClient = HttpClient.createRequest(url: url, method: .POST)
        if(httpClient == nil){
            completion(Result.failure(AuthorizationError.userNotAuthorized))
            return
        }
        if let jsonData = HttpClient.serializeObject(self){
            httpClient?.httpBody = jsonData
        }
        
        let task = URLSession.shared.dataTask(with: httpClient!){ (data, response, error) in
            
            if(!HttpClient.checkResponseAndError(response: response, error: error)){
                return
            }
            
            if let data = data {
                if let stringData = String(data: data, encoding: .utf8) {
                    if let decodedData = HttpClient.deserializeObject(stringData, type: UserData.self) {
                        print(decodedData)
                        HttpClient.token = decodedData.token
                        HttpClient.role = decodedData.role
                        completion(Result.success(true))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    //register
    public func RegisterUser(completion: @escaping (Result<Bool, Error>) -> Void){
        let url = "api/Auth/Register"
        
        var httpClient = HttpClient.createRequest(url: url, method: .POST)
        if(httpClient == nil){
            completion(Result.failure(AuthorizationError.failedToRegister))
            return
        }
        if let jsonData = HttpClient.serializeObject(self){
            httpClient?.httpBody = jsonData
        }
        
        let task = URLSession.shared.dataTask(with: httpClient!){ (data, response, error) in
            
            if(!HttpClient.checkResponseAndError(response: response, error: error)){
                return
            }
            
            if let data = data {
                if let stringData = String(data: data, encoding: .utf8) {
                    print(stringData)
                    if(stringData.elementsEqual("Succeeded")){
                        completion(Result.success(true))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    
    //register user as staff
    public func RegisterStaff(staff: AddStaff, completion: @escaping (Bool) -> Void){
        let url = "api/Staff/add?email=\(self.Username)"
        
        var httpClient = HttpClient.createRequest(url: url, method: .POST)
        if(httpClient == nil){
            completion(false)
            return
        }
        if let jsonData = HttpClient.serializeObject(staff){
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
                completion(true)
                return
            }
        }
        task.resume()
    }
    
    
    enum AuthorizationError: Error {
        case userNotAuthorized
        case failedToRegister
    }
}
    
    
struct UserData: Decodable{
        public var token:String
        public var role:String
}

struct AddStaff: Encodable{
    public var name:String
    public var phone:String
    public var dob:Date
    public var position:String
    
    init(){
        name = ""
        phone = ""
        dob = Date()
        position = "Guest"
    }
}


