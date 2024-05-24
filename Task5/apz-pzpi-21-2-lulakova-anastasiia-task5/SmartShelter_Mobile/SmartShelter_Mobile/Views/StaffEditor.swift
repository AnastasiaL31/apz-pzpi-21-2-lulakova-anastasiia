//
//  StaffEditor.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 24.05.2024.
//

import SwiftUI

struct StaffEditor: View {
    @State var accDate:Date = Date()
    @State var DOB:Date = Date()
    @State var role:String = "Guest"
    @State var dissmDate:Date?
    @Binding var staff:Staff
    public var updateStaff:(_ staff:Staff, _ role:String) ->Void
    @State var isDissmissed:Bool = false
    
    var body: some View {
        Form{
            Section(header: Text("Name")){
                TextField("", text: $staff.name)
            }
            Section(header: Text("Details")){
                HStack{
                    TextField(text:$staff.position){
                        Text("Position")
                    }
                    TextField(text:$staff.phone){
                        Text("Phone")
                    }
                }
                
                Picker("Select a Role", selection: $role) {
                    ForEach(StaffVM.roles, id: \.self) { role in
                            Text(role).tag(role)
                    }
                }
                .pickerStyle(SegmentedPickerStyle()) // You can use different styles like WheelPickerStyle(), SegmentedPickerStyle() etc.
                .padding()
                Spacer()
                DatePicker("dob", selection: $DOB, displayedComponents: .date)
                Spacer()
                DatePicker("Acc Date", selection: $accDate, displayedComponents: .date)
                if(dissmDate == nil){
                    Button(action: {
                        isDissmissed.toggle()
                    }, label: {
                        if(isDissmissed){
                            Text("Cancel")
                        }else{
                            Text("Dismiss")
                        }
                    })
                }else{
                    Text("Dissmised at \(dissmDate!)")
                    //DatePicker("Dissm Date", selection: $dissmDate!, displayedComponents: .date)
                }
                
            }
        }
        .onAppear{
            self.DOB = staff.DOB ?? Date()
            self.accDate = staff.AcceptanceDate ?? Date()
            self.dissmDate = staff.DissmisialDate
                
        }
        .onDisappear {
            staff.dob = DateConverter.dateToSwiftString(DOB)
            staff.acceptanceDate = DateConverter.dateToSwiftString(accDate)
            if isDissmissed && dissmDate == nil {
                staff.dismissialDate = DateConverter.dateToSwiftString(Date())
            }
            updateStaff(staff, role)
        }
        
    }
    
}
