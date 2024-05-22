//
//  Animal.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import Foundation

struct Animal: Codable, Hashable, Identifiable{
    
    public var id:Int
    public var name:String
    public var breed:String
    public var dob:String
    public var gender:String
    public var weight:Float
    public var acceptancedate:String?

    var DOB: Date? {
        return Animal.customDateFormatter.date(from: dob)
       }

       var AcceptanceDate: Date? {
           guard let acceptancedate = acceptancedate else { return nil }
           return Animal.customDateFormatter.date(from: acceptancedate)
       }
    
    public static func GetAllAnimals(completion: @escaping (Result<Array<Animal>, Error>) -> Void) {
        let url = "api/Animals"
        
        let httpClient = HttpClient.createRequest(url: url, method: .GET)
        if(httpClient == nil){
            completion(Result.failure(AnimalError.programError))
            return
        }
        
        let task = URLSession.shared.dataTask(with: httpClient!){ (data, response, error) in
            
            if(!HttpClient.checkResponseAndError(response: response, error: error)){
                return
            }
            
            if let data = data {
                do {
                    if let jsonString = String(data: data, encoding: .utf8) {
                            print("Received JSON: \(jsonString)")
                        }
                    let decodedData = try JSONDecoder().decode(Array<Animal>.self, from: data)
                    completion(.success(decodedData))
                } catch {
                    print("Decoding error: \(error.localizedDescription)")
                    print(error)
                    completion(.failure(error))
                }
            }
        }
        task.resume()
    }
    
    
    
    enum AnimalError: Error {
        case programError
    }
    
    private static let customDateFormatter: DateFormatter = {
           let formatter = DateFormatter()
           // Customize the date format to match your JSON date format
           formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSSZ"
           return formatter
       }()
}


