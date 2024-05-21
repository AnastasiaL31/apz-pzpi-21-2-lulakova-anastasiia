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
    
    public func GetUser(){
        var res = model.GetUser()
    }
    
    var model:LoginUser {
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
