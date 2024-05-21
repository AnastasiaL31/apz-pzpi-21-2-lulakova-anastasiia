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
    
    
    var body: some View {
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
                    userModel.GetUser()
                }
            }, label: {
                Text(sectionName)
            })
        }
    
        .padding()
        .frame(width: 300, height: 300, alignment: .center)
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
