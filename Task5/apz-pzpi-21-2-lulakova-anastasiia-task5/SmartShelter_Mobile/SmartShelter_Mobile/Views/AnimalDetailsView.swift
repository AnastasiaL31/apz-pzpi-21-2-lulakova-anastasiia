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
        ScrollView{
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
        //.ignoresSafeArea()
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
                        Text("DOB: \(dob.formatted(date: .numeric, time: .omitted))")
                    }
                    Spacer()
                    Text("Gender: \(animal.gender)")
                }
                Spacer()
                if let accDate = animal.AcceptanceDate{
                    Text("Acc Date: \(accDate.formatted(date: .numeric, time: .omitted))")
                }
                Button(action: {
                    isAnimalEditShown = true
                }, label: {
                    Text("Edit")
                })
            }
            .padding()
            .frame(maxWidth: .infinity, alignment: .leading)
            .clipped()
            .background {
                // Background Shape
                RoundedRectangle(cornerRadius: 14, style: .continuous)
                    .fill(Color(.secondarySystemGroupedBackground))
                    .shadow(color: Color(.sRGB, red: 0/255, green: 0/255, blue: 0/255).opacity(0.06), radius: 8, x: 0, y: 4)
            }
            .padding(.horizontal)
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
                    Text("Min water:\n\(formatFloatToString(aviaryCondition.minWater))")
                    Spacer()
                    Text("Food:\n\(formatFloatToString(aviaryCondition.food))")
                }.padding()
                HStack{
                    Text("Min temp:\n\(formatFloatToString(aviaryCondition.minTemperature))")
                    Spacer()
                    Text("Max temp:\n\(formatFloatToString(aviaryCondition.maxTemperature))")
                }.padding()
                HStack{
                    Text("Min humid:\n\(formatFloatToString(aviaryCondition.minHumidity))")
                    Spacer()
                    Text("Max humid:\n\(formatFloatToString(aviaryCondition.maxHumidity))")
                }.padding()
                
                Button(action: {
                    isAviaryEditShown = true
                }, label: {
                    Text("Edit")
                })
            }
            .padding()
            .frame(maxWidth: .infinity, alignment: .leading)
            .clipped()
            .background {
                
                RoundedRectangle(cornerRadius: 14, style: .continuous)
                    .fill(Color(.secondarySystemGroupedBackground))
                    .shadow(color: Color(.sRGB, red: 0/255, green: 0/255, blue: 0/255).opacity(0.06), radius: 8, x: 0, y: 4)
            }
            .padding(.horizontal)
            
    }
    
    
    var sensorView: some View{
        VStack{
            if sensor.id != 0{
                Text("Sensor #\(sensor.id)")
                Text(sensor.notes ?? " ")
                Text("Frequency: \(sensor.frequency/60000)")
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
        .padding()
        .frame(maxWidth: .infinity, alignment: .leading)
        .clipped()
        .background {
            RoundedRectangle(cornerRadius: 14, style: .continuous)
                .fill(Color(.secondarySystemGroupedBackground))
                .shadow(color: Color(.sRGB, red: 0/255, green: 0/255, blue: 0/255).opacity(0.06), radius: 8, x: 0, y: 4)
        }
        .padding(.horizontal)
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
