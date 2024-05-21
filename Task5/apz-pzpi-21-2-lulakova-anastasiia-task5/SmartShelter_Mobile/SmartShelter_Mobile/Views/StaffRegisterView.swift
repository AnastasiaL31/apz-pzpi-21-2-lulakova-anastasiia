//
//  StaffRegisterView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 21.05.2024.
//

import SwiftUI

struct StaffRegisterView: View {
    public var userModel:UserVM
    @State var addStaff: AddStaff = AddStaff()
    @State var isFunctionList = false
    
    var body: some View {
        VStack{
            Form {
                Section("Write your data"){
                    TextField("Name", text: $addStaff.name)
                    TextField("Phone", text: $addStaff.phone)
                    DatePicker("DOB", selection: $addStaff.dob, displayedComponents: .date)
                    TextField("Position", text: $addStaff.position)
                }
            }
            Button (action : {
                userModel.AddStaff(addStaff: addStaff){ result in
                    isFunctionList = result
                }
                
            }, label: {
                Text("Register")
            })
        }
        .navigationDestination(isPresented: $isFunctionList){
            FunctionListView()
        }
        .navigationBarBackButtonHidden(true)
    }
}

//#Preview {
//    StaffRegisterView(email: "")
//}
