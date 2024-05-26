//
//  StaffVM.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 24.05.2024.
//

import Foundation
import SwiftUI


class StaffVM:ObservableObject{
    
    @Published public var allStaff:Array<Staff> = []
    @Published public var selectedStaffIndex:Int?
    public static var roles: [String] = ["Admin", "Doctor", "Storekeeper", "Guest"]
    @Published var role:String = "Guest"
    
    public func getAllStaff(){
        Staff.getAllStaff{ result in
            switch result{
            case .success(var staff):
                DispatchQueue.main.async {
                    for number in 0..<staff.count {
                        staff[number].dob = DateConverter.fromServerDateToString(dateString: staff[number].dob, time: .omitted)
                        staff[number].acceptanceDate = DateConverter.fromServerDateToString(dateString: staff[number].acceptanceDate, time: .omitted)
                        if(staff[number].dismissialDate != nil){
                            staff[number].dismissialDate = DateConverter.fromServerDateToString(dateString: staff[number].dismissialDate!, time: .omitted)
                        }
                    }
                    print(staff)
                    self.allStaff = staff
                }
            case .failure(let error):
                print(error)
            }
        }
    }
    
    public func updateStaff(staff:Staff, role:String){
        var staffToUpdate = staff
        staffToUpdate.dob = DateConverter.swiftDateStringToServerString(staffToUpdate.dob)
        staffToUpdate.acceptanceDate = DateConverter.swiftDateStringToServerString(staffToUpdate.acceptanceDate)
        if(staffToUpdate.dismissialDate != nil){
            staffToUpdate.dismissialDate = DateConverter.swiftDateStringToServerString(staffToUpdate.dismissialDate!)
        }

        staffToUpdate.updateStaff(role: role){ result in
            if(result){
                self.updateRole(staff: staff, role: role)
            }else{
                print("Unsuccessful staff update")
            }
        }
    }
    
    func getStaffRole(staff: Staff, completion: @escaping (String) -> Void) {
        var role = "None"
            StaffVM.getStaffDetails(staff: staff) { result in
                switch result {
                case .success(let staffDetails):
                    role = staffDetails.role
                    completion(role)
                case .failure(let error):
                    print("Failed to get staff details: \(error)")
                    completion(role)
                }
            }
    }
    
    public static func getStaffDetails(staff:Staff, completion: @escaping (Result<StaffDetails, Error>) -> Void){
        staff.getStaffDetails(completion: completion)
    }
    
    func updateRole(staff:Staff, role:String){
        staff.updateStaffRole(role: role){ result in
            if(result){
                print("Role was updated")
            }
        }
    }
    
}

struct StaffDetails: Codable{
    public var staff:Staff
    public var role:String = "Guest"
}

