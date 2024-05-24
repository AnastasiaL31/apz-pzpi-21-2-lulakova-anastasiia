//
//  AnimalVM.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import Foundation

class AnimalVM: ObservableObject {

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
    
    public func updateAnimal(animal:Animal){
        print(animal)
        var updatedAnimal = animal
        updatedAnimal.dob = DateConverter.formatDateToString(updatedAnimal.DOB ?? Date())
        print(updatedAnimal)
        updatedAnimal.updateAnimal{result in
            if(result){
                print("Success")
            }
        }
    }
    
    public func getAnimalAviary(animalId: Int,completion: @escaping (Result<Aviary, Error>) -> Void){
        Aviary.getAnimalAviary(animalId: animalId, completion: completion)
        }
    
    public func updateAviary(aviary:Aviary){
        if(aviary.aviaryCondition != nil && aviary.aviaryCondition?.id == 0){
            var aviaryToUpdate = aviary
            aviary.addAviaryCondition {result in
                switch result{
                case .success(let id):
                    aviaryToUpdate.aviaryConditionId = id
                    aviaryToUpdate.aviaryCondition = nil
                    self.performUpdate(aviary: aviaryToUpdate)
                default:
                    return
                }
            }
        }else{
            self.performUpdate(aviary: aviary)
        }
    }
    
    private func performUpdate(aviary: Aviary) {
        // Update aviary
        aviary.updateAviary { result in
            if !result {
                print("Cannot update aviary")
            }
        }
    }
}
