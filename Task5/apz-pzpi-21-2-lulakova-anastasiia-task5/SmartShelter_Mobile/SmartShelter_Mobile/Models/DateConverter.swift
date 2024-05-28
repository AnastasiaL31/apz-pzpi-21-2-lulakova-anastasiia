//
//  DateConverter.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 24.05.2024.
//

import Foundation


class DateConverter{
    
    
    public static func formatDateToString(_ date: Date) -> String {
            let formatter = DateFormatter()
            formatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS'Z'"
            return formatter.string(from: date)
        }
    
    
    public static func createDateFromString(from dateString: String) -> Date? {
        let formatterWithMilliseconds = DateFormatter()
        formatterWithMilliseconds.dateFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS"
        
        let formatterWithoutMilliseconds = DateFormatter()
        formatterWithoutMilliseconds.dateFormat = "yyyy-MM-dd'T'HH:mm:ss"
        
        if let date = formatterWithMilliseconds.date(from: dateString) {
            return date
        } else if let date = formatterWithoutMilliseconds.date(from: dateString) {
            return date
        } else {
            return nil
        }
    }
    
    public static func fromServerDateToString(dateString:String, time:Date.FormatStyle.TimeStyle) -> String{
        if let date = createDateFromString(from: dateString){
            return date.formatted(date: .numeric, time: time)
        }
        return dateString
    }
    
    public static func fromSwiftDateStringToDate(from dateString:String) -> Date?{
        let formatter = DateFormatter()
        formatter.dateFormat = "dd/MM/yyyy"
        return formatter.date(from: dateString)
    }
    
    public static func dateToSwiftString(_ date:Date)-> String{
        return date.formatted(date: .numeric, time: .omitted)
    }
    
    
    public static func swiftDateStringToServerString(_ dateString:String) -> String{
        let date = fromSwiftDateStringToDate(from: dateString)
        return DateConverter.formatDateToString(date!)
    }
    
    public static func fromSwiftDateStringToDateWithTime(from dateString:String) -> Date?{
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "dd/MM/yyyy, HH:mm"
        dateFormatter.timeZone = TimeZone.current

        return dateFormatter.date(from: dateString)
    }
}
