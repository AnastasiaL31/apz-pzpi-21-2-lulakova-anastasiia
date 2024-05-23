//
//  AnimalDetailsView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import SwiftUI

struct AnimalDetailsView: View {
    @Binding var animal:Animal
    @State var isAnimalEditShown = false
    @State var isAviaryEditShown = false
    var animalVM:AnimalVM
    @State var animalAviary:Aviary = Aviary(id: 0, aviaryCondition: AviaryCondition(id: 0))
    @State var aviaryCondition:AviaryCondition = AviaryCondition(id: 0)
    
    
    var body: some View {
        Form{
            animalView
                .onAppear {
                    animalVM.getAnimalAviary(animalId: animal.id){result in
                        switch result{
                        case .success(let GotAviary):
                            print(GotAviary)
                            animalAviary = GotAviary
                            if(GotAviary.aviaryCondition == nil){
                                aviaryCondition = AviaryCondition(id: 0)
                            }else{
                                aviaryCondition = GotAviary.aviaryCondition!
                            }
                        case .failure(let error):
                            print(error)
                        }
                    }
                }
            Spacer()
            aviaryView
        }
        .sheet(isPresented: $isAnimalEditShown){
            let date = animal.DOB ?? Date()
            AnimalEditorView(animal: $animal,
                             updateInDB: animalVM.updateAnimal(animal:),
                             dob: date)
        }
        .sheet(isPresented: $isAviaryEditShown){
            AviaryEditor(aviary: $animalAviary,
                         updateInDB: animalVM.updateAviary(aviary:),
                         aviaryCondition: $aviaryCondition)
        }
    }
    
    var animalView: some View{
            VStack(alignment: .center){
                Text(animal.name)
                    .font(.title)
                Spacer()
                Text(animal.breed)
                    .font(.subheadline)
                HStack{
                    if let dob = animal.DOB{
                        Text("DOB: \(dob)")
                    }
                    Spacer()
                    Text("Gender: \(animal.gender)")
                }
                Spacer()
                if let accDate = animal.AcceptanceDate{
                    Text("Acc Date: \(accDate)")
                }
                Button(action: {
                    isAnimalEditShown = true
                }, label: {
                    Text("Edit")
                })
            }
        
    }
    
    
    var aviaryView: some View {
            VStack(alignment: .center){
                Text("Aviary #\(animalAviary.id)")
                    .font(.title)
                Spacer()
                if let checkedDescription = animalAviary.description{
                    Text(checkedDescription)
                        .font(.subheadline)
                }
                HStack{
                    Text("Min water: \(formatFloatToString(aviaryCondition.minWater))")
                    Spacer()
                    Text("Food: \(formatFloatToString(aviaryCondition.food))")
                }.padding()
                HStack{
                    Text("Min temp: \(formatFloatToString(aviaryCondition.minTemperature))")
                    Spacer()
                    Text("Max temp: \(formatFloatToString(aviaryCondition.maxTemperature))")
                }.padding()
                HStack{
                    Text("Min humid: \(formatFloatToString(aviaryCondition.minHumidity))")
                    Spacer()
                    Text("Max humid: \(formatFloatToString(aviaryCondition.maxHumidity))")
                }.padding()
                
                Button(action: {
                    isAviaryEditShown = true
                }, label: {
                    Text("Edit")
                })
            }
       // }
    }
    
    func formatFloatToString(_ number:Float) -> String {
        return String(format: "%.2f", number)
    }
        
}


//Admin@gmail.com
