//
//  SettingsView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 27.05.2024.
//

import SwiftUI

struct SettingsView: View {
    @AppStorage("isCelsius") private var isCelsius: Bool = true

    var body: some View {
        VStack{
            Form{
                temperatureSettings
            }
        }
        .onAppear {
                    HttpClient.isCelsius = isCelsius
                }
                .onChange(of: isCelsius) { newValue in
                    HttpClient.isCelsius = newValue
                }
        .navigationTitle("Settings")
        
    }
    
    var temperatureSettings: some View{
        Section(header: Text("Temperature Unit")) {
            Toggle(isOn: $isCelsius) {
                Text(isCelsius ? "Celsius" : "Fahrenheit")
            }
        }
    }
}

#Preview {
    SettingsView()
}
