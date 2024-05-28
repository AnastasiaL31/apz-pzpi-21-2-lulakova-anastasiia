//
//  AllStaffView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 24.05.2024.
//

import SwiftUI

struct AllStaffView: View {
    @ObservedObject var staffVM = StaffVM()
    @State private var isEditorShown = false
    
    
    
        var body: some View {
            ScrollView {
                ForEach(staffVM.allStaff) { staff in
                    if let index = staffVM.allStaff.firstIndex(where: { $0.id == staff.id }) {
                        staffItem(staff: staff, index: index)
                    }
                }
                .sheet(isPresented: $isEditorShown) {
                    if let selectedStaffIndex = staffVM.selectedStaffIndex {
                        StaffEditor(role: staffVM.role, staff: $staffVM.allStaff[selectedStaffIndex],
                                    updateStaff: staffVM.updateStaff)
                    }
                }
            }
            .onAppear {
                staffVM.getAllStaff()
            }
        }
    
    
    
    func staffItem(staff:Staff, index:Int) -> some View{
        VStack {
                Text(staff.name)
                    .font(.title)
                HStack {
                    Text("Position: \n\(staff.position)")
                    Spacer()
                    Text("Phone: \n\(staff.phone)")
                }
                HStack {
                    Text("DOB: \n\(staff.dob)")
                    Spacer()
                    Text("Acc date: \n\(staff.acceptanceDate)")
                }
            Button(action: {
                staffVM.selectedStaffIndex = index
                if let selectedStaffIndex = staffVM.selectedStaffIndex {
                    staffVM.getStaffRole(staff: staffVM.allStaff[selectedStaffIndex]){ result in
                        staffVM.role = result
                        isEditorShown = true
                    }
                }
                
            }, label: {
                Text("Edit")
            })
            }
            .padding()
            .background(staff.hasRole ? Color.white.opacity(0.3) : Color.red.opacity(0.3))
            .cornerRadius(10)
            .padding(.horizontal)
            .shadow(radius: 5)
    }
}
