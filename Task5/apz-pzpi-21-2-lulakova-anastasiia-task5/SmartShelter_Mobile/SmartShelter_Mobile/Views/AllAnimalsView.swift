//
//  AllAnimalsView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import SwiftUI

struct AllAnimalsView: View {
    @ObservedObject var animalVM:AnimalVM = AnimalVM()
    @State var animalsList: [Animal] = []
    
    
    var body: some View {
        List{
            ForEach(animalVM.animalList) {animal in
                if let index = animalVM.animalList.firstIndex(where: {$0.id == animal.id}){
                    NavigationLink(destination: AnimalDetailsView(animal: $animalVM.animalList[index])){
                        AnimalItem(animal: $animalVM.animalList[index])
                    }
                }
            }
        }
    }
        
}
    
    
    struct AnimalItem: View {
        @Binding var animal:Animal
        
        var body: some View {
                VStack{
                    Text(animal.name)
                        .fontWeight(.bold)
                    Text(animal.breed)
                    if let accDate = animal.AcceptanceDate {
                        Text(accDate.description)
                    }
                }
        }
        
    }

