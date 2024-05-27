//
//  AnimalVM.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 22.05.2024.
//

import Foundation

class AnimalVM: ObservableObject {

    @Published public var animalList:Array<Animal> = []
    let zeroKoef:Float = 32
    let slopeKoef:Float = 5/9
    
    init(){
        AnimalVM.getAllAnimals { result in
            switch result{
            case .success(var animals):
                for ind in 0..<animals.count{
                    animals[ind].acceptanceDate = DateConverter.fromServerDateToString(dateString:  animals[ind].acceptanceDate, time: .omitted)
                    animals[ind].dob = DateConverter.fromServerDateToString(dateString:  animals[ind].dob, time: .omitted)
                }
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
        updatedAnimal.dob = DateConverter.swiftDateStringToServerString(updatedAnimal.dob)
        updatedAnimal.acceptanceDate = DateConverter.formatDateToString(updatedAnimal.AcceptanceDate ?? Date())
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
    

    
    public func updateAviary(aviary:Aviary, isCels:Bool){
        if(aviary.aviaryCondition != nil){
            var aviaryToUpdate = aviary
            if(!isCels){
                convertConditionToCelsius(aviaryCondition: &aviaryToUpdate.aviaryCondition!)
            }
            if(aviary.aviaryCondition?.id == 0){
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
                self.performUpdate(aviary: aviaryToUpdate)
            }
        }
    }
    
    func convertConditionToCelsius(aviaryCondition: inout AviaryCondition){
        aviaryCondition.minTemperature = convertToCels(aviaryCondition.minTemperature)
        aviaryCondition.maxTemperature = convertToCels(aviaryCondition.maxTemperature)
    }
    
    func convertToCels(_ num:Float)->Float{
        return (num - zeroKoef)*slopeKoef
    }
    
    private func performUpdate(aviary: Aviary) {
        aviary.updateAviary { result in
            if !result {
                print("Cannot update aviary")
            }
        }
    }
    
    
    public func getAviarySensor(aviaryId:Int, completion: @escaping (Result<Sensor?, Error>) -> Void){
        Sensor.getSensor(aviaryId: aviaryId, completion: completion)
    }
    
    public func updateSensor(sensor:Sensor){
        print(sensor)
        sensor.updateSensor{result in
            if(result){
                print("Success")
            }
        }
    }
    
    public func getSensorData(sensorId:Int, completion:  @escaping (Result<Array<SensorData>, Error>) -> Void){
        SensorData.getSensorData(sensorId: sensorId, completion: completion)
    }
}
