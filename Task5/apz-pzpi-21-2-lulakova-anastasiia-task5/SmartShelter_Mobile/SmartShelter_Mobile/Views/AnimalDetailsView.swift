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
    @State var isSensorEditShown = false
    @State var isSensorDataShown = false
    var animalVM:AnimalVM
    @State var animalAviary:Aviary = Aviary(id: 0, aviaryCondition: AviaryCondition(id: 0))
    @State var aviaryCondition:AviaryCondition = AviaryCondition(id: 0)
    @State var sensor:Sensor = Sensor(id:0, frequency: 0)
    @State var sensorData: [SensorData] = []
    
    
    var body: some View {
        Form{
            animalView
                .onAppear {
                    animalVM.getAnimalAviary(animalId: animal.id){result in
                        switch result{
                        case .success(let GotAviary):
                            print(GotAviary)
                            animalAviary = GotAviary
                            getSensor()
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
            Spacer()
            sensorView
        }
        .navigationDestination(isPresented: $isSensorDataShown){
            Diagrams(sensorData: sensorData)
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
        .sheet(isPresented: $isSensorEditShown){
            SensorEditor(sensor: $sensor, updateInDB: animalVM.updateSensor(sensor:))
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
            .border(Color.black, width: /*@START_MENU_TOKEN@*/1/*@END_MENU_TOKEN@*/)
            .padding()
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
            .border(Color.black, width: /*@START_MENU_TOKEN@*/1/*@END_MENU_TOKEN@*/)
            .padding()
            
    }
    
    
    var sensorView: some View{
        VStack{
            if sensor.id != 0{
                Text("Sensor #\(sensor.id)")
                Text(sensor.notes ?? " ")
                Text("Frequency: \(sensor.frequency/3600)")
                Spacer()
                HStack{
                    Button(action: {
                        isSensorEditShown = true
                    }, label: {
                        Text("Edit")
                    })
                    Spacer()
                    Button(action: {
                        getSensorData()
                        isSensorDataShown = true
                    }, label: {
                        Text("Sensor Data")
                    })
                }
            }else{
                Text("No sensor connected to aviary")
            }
        }
        .border(Color.black, width: /*@START_MENU_TOKEN@*/1/*@END_MENU_TOKEN@*/)
        .padding()
    }
    
    func formatFloatToString(_ number:Float) -> String {
        return String(format: "%.2f", number)
    }
      
    
    func getSensor(){
        animalVM.getAviarySensor(aviaryId: animalAviary.id){ result in
            switch result{
            case .success(let sensor):
                if let unwrappedSensor = sensor{
                    self.sensor = unwrappedSensor
                }
            default:
                break
            }
        }
    }
    
    func getSensorData(){
        animalVM.getSensorData(sensorId: sensor.id){ result in
            switch result{
            case .success(var sensorData):
                for i in 0..<sensorData.count{
                    sensorData[i].date = DateConverter
                        .fromServerDateToString(dateString: sensorData[i].date, time: .shortened)
                }
                print("Sensor Data: \n \(sensorData)")
                self.sensorData = sensorData
            default:
                break
            }
        }
    }
}


//Admin@gmail.com
