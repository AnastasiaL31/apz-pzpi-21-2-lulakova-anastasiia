//
//  Order.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 26.05.2024.
//

import Foundation


struct Order: Codable, Identifiable{
    public var id:Int
    public var type:String
    public var name:String
    public var amount:Float
    public var unitOfMeasure:String
    public var price:Float
    public var isApproved:Bool
    public var orderDate:String
    public var endDate:String
    public var staffId:Int
    
    public static func getAllOrders(completion: @escaping (Result<Array<Order>, Error>) -> Void){
        let url = "api/Storage/orders/all"
        
        print("Getting all orders")
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
                    if let decodedData = HttpClient.deserializeObject(stringData, type: Array<Order>.self) {
                        print(decodedData)
                        completion(Result.success(decodedData))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    
    public func approveOrder(completion: @escaping (Bool) -> Void){
        let url = "api/Storage/approve/\(self.id)"
        
        print("Approve order")
        let httpClient = HttpClient.createRequest(url: url, method: .POST)
        if(httpClient == nil){
            completion(false)
            return
        }
        
        let task = URLSession.shared.dataTask(with: httpClient!){ (data, response, error) in
            
            if let error = error {
                print("Error: \(error)")
                completion(false)
                return
            }
            
            guard let httpResponse = response as? HTTPURLResponse else {
                print("Invalid response")
                completion(false)
                return
            }
            print(httpResponse.statusCode)
            if(httpResponse.statusCode == 200){
                print("Succesful order update")
                completion(true)
                return
            }
        }
        task.resume()
    }
}
