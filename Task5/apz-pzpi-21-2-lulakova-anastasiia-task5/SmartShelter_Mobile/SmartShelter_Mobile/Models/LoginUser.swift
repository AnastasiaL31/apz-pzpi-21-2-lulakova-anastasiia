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
    public func GetUser() ->  Bool {
        var res = false
        let url = "api/Auth/Login"
        
        var httpClient = HttpClient.createRequest(url: url, method: .POST)
        if(httpClient == nil){
            return res
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
            
            print("Status Code: \(httpResponse.statusCode)")
            
            if let data = data {
                if let stringData = String(data: data, encoding: .utf8) {
                    if let decodedData = HttpClient.deserializeObject(stringData, type: UserData.self) {
                        print(decodedData)
                        HttpClient.token = decodedData.token
                        HttpClient.role = decodedData.role
                        res = true
                    }
                }
            }
        }
        task.resume()
        return res
        }
    }
    
    
struct UserData: Decodable{
        public var token:String
        public var role:String
}


