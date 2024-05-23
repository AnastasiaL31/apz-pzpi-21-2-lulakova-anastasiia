//
//  HttpClient.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 20.05.2024.
//

import Foundation

class HttpClient {
    public static var backendAddress = "http://192.168.1.9:5139/"
    public static var token = ""
    public static var role = ""
    
    public enum HTTPMethod: String {
        case GET
        case POST
        case DELETE
        case PUT
    }
    
    public static func createRequest(url: String, method:HTTPMethod) -> URLRequest?{
        if let endURL = URL(string: backendAddress + url){
            var request = URLRequest(url: endURL)
            request.httpMethod = method.rawValue
            if(!HttpClient.token.isEmpty){
                request.addValue("Bearer \(token)", forHTTPHeaderField: "Authorization")
            }
            if(method == .POST || method == .PUT){
                request.setValue("application/json", forHTTPHeaderField: "Content-Type")
            }
            return request
        }
        return nil
    }
    
    public static func serializeObject<T: Encodable>(_ object: T) -> Data? {
        let encoder = JSONEncoder()
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"
        encoder.keyEncodingStrategy = .useDefaultKeys
        encoder.dateEncodingStrategy = .formatted(dateFormatter)
        
        do {
            let jsonData = try encoder.encode(object)
            return jsonData
        } catch {
            print("Error: \(error)")
            return nil
        }
    }
    
    public static func deserializeObject<T: Decodable>(_ jsonString: String, type: T.Type) -> T? {
        guard let jsonData = jsonString.data(using: .utf8) else {
            return nil
        }
        let decoder = JSONDecoder()
        
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"
        decoder.dateDecodingStrategy = .iso8601
        //decoder.dateDecodingStrategy = .formatted(dateFormatter)
        decoder.keyDecodingStrategy = .convertFromSnakeCase
        do {
            let decodedObject = try decoder.decode(type, from: jsonData)
            return decodedObject
        } catch {
            print("Error: \(error)")
            return nil
        }
    }

    
    public static func checkResponseAndError(response:URLResponse?, error:Error?) -> Bool{
        if let error = error {
            print("Error: \(error)")
            return false
        }
        
        guard let httpResponse = response as? HTTPURLResponse else {
            print("Invalid response")
            return false
        }
        
        print("Status Code: \(httpResponse.statusCode)")
        
        return true
    }
    
    
    public static func formatDate(_ date: Date) -> String {
            let formatter = DateFormatter()
            formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"
            return formatter.string(from: date)
        }
}

enum RequestError: Error{
    case WrongRequest
    case BadRequest
    case WrongData
}

