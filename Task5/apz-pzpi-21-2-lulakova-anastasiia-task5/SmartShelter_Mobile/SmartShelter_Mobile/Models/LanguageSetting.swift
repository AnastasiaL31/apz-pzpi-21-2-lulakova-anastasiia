//
//  LanguageSetting.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 28.05.2024.
//

import Foundation

@Observable
class LanguageSetting:ObservableObject {
   var locale: Locale = Locale(identifier: "en")

    init() {
            let lang = UserDefaults.standard.string(forKey: "language") ?? "en"
            self.locale = Locale(identifier: lang)
        }
    
    public func changeLang(identifier:String){
        self.locale = Locale(identifier: identifier)
        UserDefaults.standard.set(identifier, forKey: "language")
    }
    
    static var languages = ["English", "Українська"]
}
