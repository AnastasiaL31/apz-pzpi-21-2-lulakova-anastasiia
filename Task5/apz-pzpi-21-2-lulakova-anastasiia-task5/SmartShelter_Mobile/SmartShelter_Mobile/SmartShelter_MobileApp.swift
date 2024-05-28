//
//  SmartShelter_MobileApp.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 20.05.2024.
//

import SwiftUI

@main
struct SmartShelter_MobileApp: App {
    @State var languageSettings = LanguageSetting()
    
    init() {
           HttpClient.isCelsius = UserDefaults.standard.bool(forKey: "isCelsius")
       }
    
    
    var body: some Scene {
        WindowGroup {
            ContentView()
                .environment(languageSettings)
                .environment(\.locale, languageSettings.locale)
        }
    }
}
