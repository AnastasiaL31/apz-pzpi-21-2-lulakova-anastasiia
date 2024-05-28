//
//  Diagrams.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 25.05.2024.
//

import SwiftUI
import Charts


struct Diagrams: View {
    //@Binding
    var sensorData:Array<SensorData>
    @State var isCels = HttpClient.isCelsius
    var zeroKoef:Float = 32
    var slopeKoef:Float = 1.8
    
    
    var body: some View {
        VStack{
            Text("Temperature")
                .padding()
            Chart{
                ForEach(sensorData) {data in
                    let dataFormat = sensorData.firstIndex(where: {$0.id == data.id})
                    == sensorData.firstIndex(
                        where: {areDatesEqual(
                            DateConverter.fromSwiftDateStringToDateWithTime(from: $0.date)!,
                            DateConverter.fromSwiftDateStringToDateWithTime(from:data.date)!)}
                    ) ? Date.FormatStyle.DateStyle.numeric : Date.FormatStyle.DateStyle.omitted
                    if let date = DateConverter.fromSwiftDateStringToDateWithTime(from: data.date){
                        var temperature = isCels ? data.temperature : data.temperature*slopeKoef + zeroKoef
                        BarMark(x: .value("Date", date, unit: .minute), y: .value("Temp", temperature), width: 50)
                            .annotation(position:.overlay){
                                Text(formatFloatToString(temperature) + "\n\(date.formatted(date:dataFormat , time: .shortened))")
                                    .font(.caption2)
                            }
                    }
                }
            }
            .padding()
        
        }
        .onAppear{
                print(sensorData)
        }
    }
    
    func formatFloatToString(_ number:Float) -> String {
        return String(format: "%.0f", number)
    }
    
    func areDatesEqual(_ date1: Date, _ date2: Date) -> Bool {
        let calendar = Calendar.current

        let components1 = calendar.dateComponents([.year, .month, .day], from: date1)
        let components2 = calendar.dateComponents([.year, .month, .day], from: date2)

        return components1.year == components2.year &&
               components1.month == components2.month &&
               components1.day == components2.day
    }
}




