//
//  AnimalDetailsView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import SwiftUI

struct AnimalDetailsView: View {
    @Binding var animal:Animal
    
    
    var body: some View {
        Text(animal.name)
        Button(action: {
            animal.name = "\(animal.name)22"
        }, label: {
            /*@START_MENU_TOKEN@*/Text("Button")/*@END_MENU_TOKEN@*/
        })
    }
}


