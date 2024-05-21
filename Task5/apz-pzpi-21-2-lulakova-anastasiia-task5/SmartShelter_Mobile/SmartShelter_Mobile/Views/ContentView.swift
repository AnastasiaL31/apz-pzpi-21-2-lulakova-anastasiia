//
//  ContentView.swift
//  SmartShelter_Mobile
//
//  Created by Anastasia Lulakova on 20.05.2024.
//

import SwiftUI

struct ContentView: View {
    @ObservedObject var userModel = UserVM()
    @State private var sectionName = "Login"
    @State var isLogin = true
    @State var isRegister = false
    @State var destination:Bool  = false
    @State private var navigateToStaffRegister = false
    @State private var navigateToAnotherView = false
    
    var body: some View {
        NavigationStack {
            VStack {
                HStack{
                    Button(action: {
                        changeSheets()
                    }, label: {
                        Text("Login")
                    })
                    
                    Button(action: {
                        changeSheets()
                    }, label: {
                        Text("Register")
                    })
                }
                Divider()
                login
                Button (action: {
                    if(isLogin){
                        userModel.AuthorizeUser { result in
                            switch result {
                            case .success(true):
                                navigateToAnotherView = true
                            default:
                                print("Error")
                            }
                        }
                        }else{
                        userModel.RegisterUser { result in
                            switch result {
                            case .success(true):
                                navigateToStaffRegister = true
                            default:
                                print("Error")
                            }
                        }
                    }
                }, label: {
                    Text(sectionName)
                })
            }
            .navigationDestination(isPresented: $navigateToStaffRegister) {
                StaffRegisterView(userModel: userModel)
            }
            .navigationDestination(isPresented: $navigateToAnotherView) {
                FunctionListView()
            }
        }
        .padding()
        //.frame(width: 300, height: 400, alignment: .center)
    }
    
    
    var login: some View{
        Form{
            Section(sectionName) {
                TextField("Username",
                          text: $userModel.username)
                TextField("Password",
                          text: $userModel.password)
            }
            
        }
    }
    
    private func changeSheets(){
        isLogin.toggle()
        isRegister.toggle()
        if(sectionName == "Login"){
            sectionName = "Register"
        }else{
            sectionName = "Login"
        }
    }
}

#Preview {
    ContentView()
}
