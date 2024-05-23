//
//  AnimalEditorView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 23.05.2024.
//

import SwiftUI

struct AnimalEditorView: View {
    @Binding var animal:Animal
    var updateInDB: (Animal) -> Void
    @State var dob:Date
    
    
    var body: some View {
        Form {
            Section(header: Text("Name")){
                TextField("Name", text: $animal.name)
            }
            Section(header: Text("Details")){
                HStack{
                    TextField("Breed", text: $animal.breed)
                    TextField("Gender", text: $animal.gender)
                }
                HStack{
                    DatePicker(selection: $dob){
                        Text("DOB")
                    }
                    TextField("Enter your score", value: $animal.weight, formatter: formatter)
                    .textFieldStyle(RoundedBorderTextFieldStyle())
                    .padding()
                }
                
            }
        }
        .onDisappear {
            animal.dob = dob.formatted()
            updateInDB(animal)
        }
    }
    
    let formatter: NumberFormatter = {
            let formatter = NumberFormatter()
            formatter.numberStyle = .decimal
            return formatter
        }()
}

