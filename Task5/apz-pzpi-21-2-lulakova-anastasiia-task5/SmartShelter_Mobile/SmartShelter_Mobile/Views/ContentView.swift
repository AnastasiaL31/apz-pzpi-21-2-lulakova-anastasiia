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
            VStack (spacing: 15){
                HStack(spacing: 80){
                    Button(action: {
                        changeSheets()
                    }, label: {
                        Text("Login")
                            .font(.title3)
                            .foregroundStyle(.blue)
                    })
                    
                    Button(action: {
                        changeSheets()
                    }, label: {
                        Text("Register")
                            .font(.title3)
                            .foregroundStyle(.blue)
                    })
                }
                .padding()
                Divider()
                
                
                VStack(spacing:3){
                    Text(sectionName == "Login" ? "Login" : "Register")
                        .font(.title3)
                        .fontWeight(.bold)
                        .padding()
                        .foregroundStyle(.cyan)
                    login
                       .padding(0)
                }
                .background {
                    RoundedRectangle(cornerRadius: 14, style: .circular)
                        .foregroundStyle(.white)
                        .shadow(color: .black.opacity(0.3), radius: 8, x: 0, y: 4)
                        }
                .frame(width: 300, height:270, alignment: .leading)
                .padding()
                
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
                    Text(sectionName == "Login" ? "Login" : "Register")
                        
                })
                .padding()
                .frame(width: 250, height: 40, alignment: .center)
                .foregroundStyle(.white)
                .background {
                            RoundedRectangle(cornerRadius: 4, style: .continuous)
                        .fill(.blue)
                        }
            }
            .navigationDestination(isPresented: $navigateToStaffRegister) {
                StaffRegisterView(userModel: userModel)
            }
            .navigationDestination(isPresented: $navigateToAnotherView) {
                FunctionListView()
            }
        }
        .ignoresSafeArea()
        .background(Color(red: 255, green: 250, blue: 237))
        .padding()
    }
    
    
    var login: some View{
        Form{
            VStack(spacing:35){
                Section{
                    TextField("Username",
                              text: $userModel.username)
                }
               
                Section{
                    SecureField("Password",
                              text: $userModel.password)
                }
            }
            .padding()
        }
        
        .multilineTextAlignment(.center)
       
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
