//
//  UserVM.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 20.05.2024.
//

import Foundation

class UserVM:ObservableObject {
    @Published private var user:LoginUser

    init() {
        self.user = LoginUser(Username: "", Password: "")
    }
    
    public func AuthorizeUser(completion: @escaping (Result<Bool, Error>) -> Void) {
        model.GetUser(completion: completion)
    }
    
    public func RegisterUser(completion: @escaping (Result<Bool, Error>) -> Void){
        model.RegisterUser {result in
            switch result {
            case .success(true):
                self.model.GetUser(completion: completion)
            default:
                return
            }
        }
    }
    
    public func AddStaff(addStaff: AddStaff, completion: @escaping (Bool) -> Void){
        model.RegisterStaff(staff: addStaff, completion: completion)
    }
    
    public var model:LoginUser {
        return user
    }
    
    var username:String{
        set{
            user.Username = newValue
        }get{
            return model.Username
        }
    }
    
    var password:String{
        set{
            user.Password = newValue
        }get{
            return model.Password
        }
    }
}
