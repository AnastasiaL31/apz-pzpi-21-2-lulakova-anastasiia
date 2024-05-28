//
//  SettingsView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 27.05.2024.
//

import SwiftUI



struct SettingsView: View {
    @AppStorage("isCelsius") private var isCelsius: Bool = true
    @AppStorage("language") private var language = ""
    @Environment(LanguageSetting.self) var languageSettings

    var body: some View {
        VStack{
            Form{
                temperatureSettings
                languageSetting
            }
        }
        .onAppear {
            HttpClient.isCelsius = isCelsius
            language = languageSettings.locale.identifier
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
    
    var languageSetting: some View {
            Section(header: Text("Language")) {
                Picker("Select a Language", selection: $language) {
                    ForEach(LanguageSetting.languages, id: \.self) { lang in
                        Text(lang).tag(lang == "English" ? "en" : "uk")
                    }
                }
                .pickerStyle(SegmentedPickerStyle())
                .onChange(of: language){
                    languageSettings.changeLang(identifier: language)
                }
            }
        }
}

#Preview {
    SettingsView()
}
