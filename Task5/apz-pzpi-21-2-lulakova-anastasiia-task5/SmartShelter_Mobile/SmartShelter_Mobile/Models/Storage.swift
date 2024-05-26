//
//  Storage.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 26.05.2024.
//

import Foundation

struct Storage: Codable, Identifiable{
    public var id:Int
    public var type:String
    public var name:String
    public var amount:Float
    public var unitOfMeasure:String
    public var price:Float
    public var date:String
    public var staffId:Int
    
    init(order:Order){
        id = order.id
        type = order.type
        name = order.name
        amount = order.amount
        unitOfMeasure = order.unitOfMeasure
        price = order.price
        date = order.orderDate
        staffId = order.staffId
    }
    
    public static func getFullStorage(completion: @escaping (Result<Array<Storage>, Error>) -> Void){
        let url = "api/Storage/all"
        
        print("Getting full storage")
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
                    if let decodedData = HttpClient.deserializeObject(stringData, type: Array<Storage>.self) {
                        print(decodedData)
                        completion(Result.success(groupStorage(decodedData)))
                        return
                    }
                }
            }
        }
        task.resume()
    }
    
    public static func groupStorage(_ storage:Array<Storage>) -> Array<Storage>{
        var groupedStorage:[Storage] = []
        for ind in 0..<storage.count{
            if let index = groupedStorage.firstIndex(where: {$0.name.lowercased() == storage[ind].name.lowercased()}){
                groupedStorage[index].amount += storage[ind].amount
                groupedStorage[index].price += storage[ind].price
            }else{
                groupedStorage.append(storage[ind])
            }
        }
        print("Grouped Storage:\n\(groupedStorage)")
        return groupedStorage
    }
    
}
