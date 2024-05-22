//
//  AnimalVM.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import Foundation

class AnimalVM: ObservableObject {
    @Published public var selectedAnimal:Animal?
    @Published public var animalList:Array<Animal> = []
    
    
    init(){
        AnimalVM.getAllAnimals { result in
            switch result{
            case .success(let animals):
                self.animalList = animals
            default:
                break
            }
        }
    }

    public static func getAllAnimals(completion: @escaping (Result<Array<Animal>, Error>) -> Void){
        Animal.GetAllAnimals(completion: completion)
    }
}
