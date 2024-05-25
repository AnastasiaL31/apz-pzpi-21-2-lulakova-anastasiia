//
//  SensorEditor.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 25.05.2024.
//

import SwiftUI

struct SensorEditor: View {
    @Binding var sensor:Sensor
    var updateInDB: (Sensor) -> Void
    @State var description = ""
    
    var body: some View {
        Form {
            Section(header: Text("Notes")){
                TextField("Description", text: $description)
            }
            
            Section(header: Text("Frequency")){
                NumberTextField(value: $sensor.frequency, name: "Min water")
            }
        }
        .onAppear{
            sensor.frequency /= 60000
        }
        .onDisappear {
            sensor.notes = description
            sensor.frequency *= 60000
            updateInDB(sensor)
        }
    }
    
    func NumberTextField(value: Binding<Int>, name:String) -> some View{
        TextField(name, value: value, formatter: formatter)
        .textFieldStyle(RoundedBorderTextFieldStyle())
        .padding()
    }
    
    let formatter: NumberFormatter = {
            let formatter = NumberFormatter()
            formatter.numberStyle = .decimal
            return formatter
        }()
}



