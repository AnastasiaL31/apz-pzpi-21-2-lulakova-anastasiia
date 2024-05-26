//
//  AviaryEditor.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 23.05.2024.
//

import SwiftUI

struct AviaryEditor: View {
    @Binding var aviary:Aviary
    var updateInDB: (Aviary) -> Void
    @State var description = ""
    @Binding var aviaryCondition:AviaryCondition
    
    var body: some View {
        Form {
            Section(header: Text("Name")){
                TextField("Description", text: $description)
            }
            .padding()
            Section(header: Text("Conditions")){
                HStack{
                    VStack(spacing:3){
                        Text("Min water")
                            .font(.subheadline)
                        NumberTextField(value: $aviaryCondition.minWater, name: "Min water")
                    }
            
                    VStack{
                        Text("Food")
                            .font(.subheadline)
                        NumberTextField(value: $aviaryCondition.food, name:"Food")
                    }
                }
               
                HStack{
                    VStack(spacing:3){
                        Text("Min temperature")
                            .font(.subheadline)
                        NumberTextField(value: $aviaryCondition.minTemperature, name: "Min temperature")
                    }
                    
                    VStack(spacing:3){
                        Text("Max temperature")
                            .font(.subheadline)
                        NumberTextField(value: $aviaryCondition.maxTemperature, name: "Max temperature")
                    }
                }
                
                HStack{
                    VStack(spacing:3){
                        Text("Min Humidity")
                            .font(.subheadline)
                        NumberTextField(value: $aviaryCondition.minHumidity, name: "Min Humidity")
                    }
                    VStack(spacing:3){
                        Text("Max Humidity")
                            .font(.subheadline)
                        NumberTextField(value: $aviaryCondition.maxHumidity, name: "Max Humidity")
                    }
                }
            }
        }
        .onDisappear {
            aviary.description = description
            aviary.aviaryCondition = aviaryCondition
            updateInDB(aviary)
        }
    }
    
    func NumberTextField(value: Binding<Float>, name:String) -> some View{
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

